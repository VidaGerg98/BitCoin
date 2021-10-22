using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BitCoin
{
    class Account
    {
        public string Name;
        public RSAParameters _privateKey;
        public RSAParameters _publicKey;

        public Account(string name)
        {
            RSA rsa = RSA.Create();
            Name = name;
            _privateKey = rsa.ExportParameters(true);
            _publicKey = rsa.ExportParameters(false);
        }

        public string GetName()
        {
            return Name;
        }
        
        public RSAParameters GetPrivateKey()
        {
            return _privateKey;
        }

        public RSAParameters GetPublicKey()
        {
            return _publicKey;
        }
    }
}
