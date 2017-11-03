using Neo;
using Neo.Wallets;
using Neo.Cryptography;
using Neo.Cryptography.ECC;
using System;
using System.Linq;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var decode = Base58.Decode("AdistributedSmartEconomyNetW1rk112");
            //var decode = Base58.Decode("ANtsharesDigitaLAssetsForYou911111");
            //var decode = Base58.Decode("AGreetingForTheHorde11111111111111");
            //var decode = Base58.Decode("AbrotheLBiLLForPeterLin88888888888");
            //var decode = Base58.Decode("Aaiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");

            var data = decode.Take(decode.Length - 4);
            var hash = data.Concat(data.Sha256().Sha256().Take(4)).ToArray();
            var address = Base58.Encode(hash);
            try
            {
                ToScriptHash(address);
                Console.WriteLine(address);
            }
            catch (FormatException)
            {
                Console.WriteLine("error");
            }

            Console.ReadLine();
        }

        public static string PublicKeyToSignatureContractAddress(string publicKey)
        {
            ECPoint cyanPubKey;
            ECPoint.TryParse(publicKey, ECCurve.Secp256r1, out cyanPubKey);
            return VerificationContract.CreateSignatureContract(cyanPubKey).Address;
        }

        public static UInt160 ToScriptHash(string address)
        {
            byte[] data = Base58.Decode(address);
            if (data.Length != 25)
                throw new FormatException();
            if (data[0] != 23)
                throw new FormatException();
            if (!data.Take(21).Sha256().Sha256().Take(4).SequenceEqual(data.Skip(21)))
                throw new FormatException();
            return new UInt160(data.Skip(1).Take(20).ToArray());
        }
    }
}
