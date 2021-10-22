using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace BitCoin
{
    class Program
    {
        static List<Block> BlockChain = new List<Block>();
        static List<Transaction> Transactions = new List<Transaction>();
        static List<string> TransactionHashList = new List<string>();
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Account Bob = new Account("Bob");
            Account Alice = new Account("Alice");
            byte[] signedTransaction;

            for (int j = 0; j < 2; j++)
            {
                Console.WriteLine("Transactions in Block:");
                for (int i = 0; i < 4; i++)
                {
                    int coinAmount = rnd.Next(1, 100);
                    Transaction t = new Transaction(Bob, Alice, coinAmount);
                    Transactions.Add(t);
                    signedTransaction = HashAndSignBytes(Encoding.UTF8.GetBytes(t.ToString()), t.From._privateKey);
                    TransactionHashList.Add(CalculateHash(BytesToString(signedTransaction)));
                    Console.WriteLine(t);
                }

                GenerateBlock();

                Transactions.Clear();
                TransactionHashList.Clear();
            }

            foreach (var item in BlockChain)
            {
                Console.WriteLine("---------------");
                Console.WriteLine(item);                
            }

            Console.ReadKey();
        }

        private static void GenerateBlock()
        {
            TransactionHashList = GenerateMerkleTree(TransactionHashList);
            var lastBlock = BlockChain.LastOrDefault();
            var block = new Block(lastBlock?.Index + 1 ?? 0, 0, DateTime.Now, "", lastBlock?.Hash ?? "0000", TransactionHashList);
            MineBlock(block);
            BlockChain.Add(block);
        }

        private static void MineBlock(Block block)
        {
            var merkleRootHash = block.MerkleTree.First();
            long nounce = -1;
            var hash = string.Empty;
            do
            {
                nounce++;
                var rowData = block.Index + block.PreviousHash + block.TimeStamp.ToString() + nounce + merkleRootHash;
                hash = CalculateHash(rowData);
            }
            while (!hash.StartsWith("0000"));
            block.Hash = hash;
            block.Nounce = nounce;
        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                return RSAalg.SignData(DataToSign, SHA256.Create());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static string CalculateHash(string rawData)
        {
            string str;
            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            str = BytesToString(bytes);

            return str;
        }

        public static string BytesToString(byte[] bytes)
        {
            string str = string.Empty;
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            str = builder.ToString();

            return str;
        }

        public static List<string> GenerateMerkleTree(List<string> merkleNodes)
        {
            List<string> merkleTree = new List<string>();
            List<string> tempMerkleTree = new List<string>();
            List<string> tempParents = new List<string>();
            List<string> tempNodes = new List<string>();
            string temp;
            int nodeCount;

            foreach (var item in merkleNodes)
            {

                merkleTree.Add(item);
            }

            tempNodes.AddRange(merkleTree);

            nodeCount = Convert.ToInt32(Math.Ceiling(tempNodes.Count / 2.0));
            while (nodeCount != 1)
            {
                nodeCount = Convert.ToInt32(Math.Ceiling(tempNodes.Count / 2.0));
                for (int i = 0; i < nodeCount; i++)
                {
                    try
                    {
                        temp = CalculateHash(tempNodes.ElementAt(2 * i) + tempNodes.ElementAt(2 * i + 1));
                    }
                    catch
                    {
                        temp = CalculateHash(tempNodes.ElementAt(2 * i));
                    }
                    tempParents.Add(temp);
                }

                tempMerkleTree.Clear();
                tempMerkleTree.AddRange(merkleTree);
                merkleTree.Clear();
                merkleTree.AddRange(tempParents);
                merkleTree.AddRange(tempMerkleTree);
                tempNodes.Clear();
                tempNodes.AddRange(tempParents);
                tempParents.Clear();
            }

            return merkleTree;
        }
    }
}
