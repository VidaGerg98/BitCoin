using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BitCoin
{
    class Block
    {
        public int Index;
        public long Nounce;
        public DateTime TimeStamp;
        public string Hash;
        public string PreviousHash;
        public List<string> MerkleTree = new List<string>();

        public Block(int index, long nounce, DateTime timeStamp, string hash, string previousHash, List<string> transactionList)
        {
            Index = index;
            Nounce = nounce;
            TimeStamp = timeStamp;
            Hash = hash;
            PreviousHash = previousHash;
            MerkleTree.AddRange(transactionList);
        }

        public override string ToString()
        {
            string str = $"Index: {Index}\nNounce: {Nounce}\nTime Stamp: {TimeStamp}\nBlock Hash: {Hash}\nPrevios Block Hash: {PreviousHash}\nMerkel Tree Root: {MerkleTree.First()}";
            return str;
        }
    }
}
