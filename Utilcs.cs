using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LanchesMac
{
    public class Util
    {
        public enum TipoCriptografia
        {
            Nenhuma = 0,
            SHA1 = 1
        }


        public static string EncodeTo64(string toEncode)

        {

            byte[] toEncodeAsBytes

                = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }


        public static bool VerifyHash(string input, string hash)
        {

            // Hash the input.
            string hashOfInput = GetHash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static string GetHash(string input)
        {

            SHA256 sha256Hash = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string DecodeFrom64(string encodedData)

        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

                System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }

        public static string GenerateHash(string pwd, string saltAsBase64)
        {
            byte[] p1 = Convert.FromBase64String(saltAsBase64);
            return GenerateHash(pwd, p1);
        }

        public static string GenerateHash(string pwd, byte[] saltAsByteArray)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] p1 = saltAsByteArray;
            byte[] p2 = System.Text.Encoding.Unicode.GetBytes(pwd);

            byte[] data = new byte[p1.Length + p2.Length];

            p1.CopyTo(data, 0);
            p2.CopyTo(data, p1.Length);

            byte[] result = sha.ComputeHash(data);

            string res = Convert.ToBase64String(result);
            return res;
        }

        public static string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string EncodePassword(string pass, TipoCriptografia passwordFormat, string salt)
        {
            if (passwordFormat == TipoCriptografia.Nenhuma) // MembershipPasswordFormat.Clear
            {
                return pass;
            }

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            if (passwordFormat == TipoCriptografia.SHA1)
            { // MembershipPasswordFormat.Hashed
                HashAlgorithm s = HashAlgorithm.Create("SHA1");
                // Hardcoded "SHA1" instead of Membership.HashAlgorithmType
                bRet = s.ComputeHash(bAll);
            }

            return Convert.ToBase64String(bRet);
        }


        public static IConfigurationBuilder GetConfiguration()
        {
            try
            {
                try
                {
                    //NetCore 2.1-
                    IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);

                    return builder;
                }
                catch (Exception)
                {
                    //Implementação para netCore 2.2+
                    IConfigurationBuilder builder = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);

                    return builder;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static string GetConfiguration(string key)
        {

            return GetConfiguration().Build()[key];
        }




        public static string GetParameterByKey(string parameter)
        {
            try
            {
                return GetConfiguration($"Parameters:{parameter}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetConnectionString(string key)
        {
            try
            {
                return GetConfiguration("connectionString:" + key);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string AjustaEspecialCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                {
                    sb.Append(text[i]);
                }
                else
                {
                    sb.Append(s_Diacritics[text[i]]);
                }
            }

            return sb.ToString();
        }

        public static string RemoveSpecialCharacters(string text, bool allowSpace = false, bool allowDot = false)
        {
            string ret;

            if (allowDot && allowSpace)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZãÃéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s\./]+?", string.Empty);
            }
            else if (allowDot)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZãÃéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\.]+?", string.Empty);
            }
            else if (allowSpace)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZãÃéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
            }
            else
            {
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZãÃéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);
            }

            return ret;
        }

        private static readonly char[] s_Diacritics = GetDiacritics();
        private static char[] GetDiacritics()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
            {
                accents[i] = (char)i;
            }

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }


    }
}

