using System;
using System.Text;
using System.Security.Cryptography;

namespace SimplePassword
{
    public class SaltedPasswordHash
    {
        private readonly string _hash;
        private readonly string _salt;
        private readonly HashAlgorithm _algorithm;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly int _saltSize;

        private const int DefaultSaltSize = 512;

        public string Hash { get { return _hash; } }
        public string Salt { get { return _salt; } }

        public SaltedPasswordHash(string password, int saltSize = DefaultSaltSize, HashAlgorithm algorithm = null, RandomNumberGenerator randomNumberGenerator = null)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (algorithm == null)
                _algorithm = new SHA512Managed();
            else
                _algorithm = algorithm;

            if (randomNumberGenerator == null)
                _randomNumberGenerator = new RNGCryptoServiceProvider();
            else
                _randomNumberGenerator = randomNumberGenerator;

            _saltSize = saltSize;

            _salt = CreateSalt();
            _hash = CreateHash(password);
        }

        public SaltedPasswordHash(string hash, string salt)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            if (salt == null)
                throw new ArgumentNullException("salt");

            _hash = hash;
            _salt = salt;
            _saltSize = DefaultSaltSize;
        }

        private string CreateSalt()
        {
            byte[] saltBuffer = new byte[_saltSize];
            _randomNumberGenerator.GetNonZeroBytes(saltBuffer);
            return Convert.ToBase64String(saltBuffer);
        }

        private string CreateHash(string password)
        {
            byte[] saltAndPassword = Encoding.Unicode.GetBytes(String.Concat(_salt, password));
            byte[] hashBuffer = _algorithm.ComputeHash(saltAndPassword);
            return Encoding.Unicode.GetString(hashBuffer);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var otherHash = obj as SaltedPasswordHash;
            if (otherHash == null)
                return false;

            return Hash.Equals(otherHash.Hash);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public static bool operator ==(SaltedPasswordHash first, SaltedPasswordHash second)
        {
            if (System.Object.ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(SaltedPasswordHash first, SaltedPasswordHash second)
        {
            return !(first == second);
        }
    }
}
