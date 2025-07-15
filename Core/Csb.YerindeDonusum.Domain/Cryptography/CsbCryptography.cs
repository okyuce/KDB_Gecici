using System.Security.Cryptography;
using System.Text;

namespace Csb.YerindeDonusum.Domain.Cryptography;


public static class CsbCryptography
{
    /// <summary>
    /// http-s://jintechflow.wordpress.com/2021/06/28/hash-algorithm-in-net-core/
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static string Sha256(string rawData)
    {
        string result = string.Empty;
        
        using (var myHash = SHA256Managed.Create())
        {
            /*
             * 2. Invoke the ComputeHash method by passing 
                  a byte array. 
             *    Just remember, you can pass any raw data, 
                  and you need to convert that raw data 
                  into a byte array.
             */
            var byteArrayResultOfRawData = 
                Encoding.UTF8.GetBytes(rawData);
 
            /*
             * 3. The ComputeHash method, after a successful 
                  execution it will return a byte array, 
             *    and you should store that in a variable. 
             */
 
            var byteArrayResult = 
                myHash.ComputeHash(byteArrayResultOfRawData);
 
            /*
             * 4. After the successful execution of ComputeHash, 
                  you can then convert 
                  the byte array into a string. 
             */
 
            result = 
                string.Concat(Array.ConvertAll(byteArrayResult, 
                    h => h.ToString("X2")));
        }

        return result;
    }
    
    /// <summary>
    /// http-s://www.technologycrowds.com/2019/09/how-to-compute-md5-hash-message-digest.html
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static string MD5(string rawData)
    {
        StringBuilder hash = new StringBuilder();

        // defining MD5 object
        using (var md5provider = new MD5CryptoServiceProvider())
        {
            // computing MD5 hash
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(rawData));
            
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
        }
        
        return hash.ToString();
    }
}