using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Models;
using _750HrsTracker.Services.Implementations;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace _750HrsTracker.Helpers
{
    public class Utility
    {
        public static string GetMimeType(string fileName)
        {
            // Make Sure Microsoft.AspNetCore.StaticFiles Nuget Package is installed
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType!))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public static string ValidateAndFixUrl(string inputUrl)
        {
            // Check if the URL starts with "http://" or "https://"
            if (inputUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || inputUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // URL already has a valid protocol, no modification needed
                return inputUrl;
            }
            else
            {
                // Append "https://" to the URL
                if (!inputUrl.StartsWith("//"))
                {
                    inputUrl = "//" + inputUrl;
                }
                return "https:" + inputUrl;
            }
        }
        public static StringBuilder UcWords(string theString)
        {
            StringBuilder output = new StringBuilder();
            string[] pieces = theString.Split(' ');
            foreach (string piece in pieces)
            {
                char[] theChars = piece.ToCharArray();
                theChars[0] = char.ToUpper(theChars[0]);

                output.Append(new string(theChars));
                output.Append(' ');
            }

            return output.Remove(output.Length - 1, 1);
        }



        public static Tuple<string, bool> GetUserIdFromToken(HttpRequest httpRequest)
        {
            var principal = httpRequest.HttpContext.User;
            string? userId = null;

            var isAdmin = false;          
            
            if (principal.HasClaim(c => c.Type == "Id") && principal.Identity!.IsAuthenticated)
            {

                userId = principal.Claims.First(x => x.Type == "Id").Value;

            }

            return Tuple.Create(userId!, isAdmin);
        }

        public static string GenerateJwtToken(UserUtilData userUtilData, AppSettings appSettings, string authPolicy)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret); GeM$secr!t
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret!);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", userUtilData.Id.ToString()),
                    new Claim(ClaimTypes.UserData, userUtilData.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, appSettings.AppBaseUrl!),
                    new Claim(JwtRegisteredClaimNames.Aud, appSettings.AppBaseUrl!),
                    new Claim("UserType", authPolicy!),
                }),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static bool ValidatePercentage(decimal value)
        {
            bool valid = false;
            if (value == 0)
            {
                return true;
            }

            decimal decValue = value / 100;
            if (decValue <= 1 && decValue > 0)
            {
                valid = true;
            }

            return valid;
        }

        public static Dictionary<string, string> GetUniqueIdValiationRequestHeader(string uniqueId, string passcode, string secretKey)
        {
            string data = $"{uniqueId}{passcode}{secretKey}";
            string encryptedkey = Encryption.SHA256(data);
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("x-uid-validate", encryptedkey);

            return header;

        }

        public static string GenerateNewDocumentCode(string docPrefix, string lastDocumentCode)
        {

            if (lastDocumentCode == null)
            {
                return docPrefix + (1.ToString("D5"));

            }

            string code = lastDocumentCode.Substring(docPrefix.Length).Trim();

            int numberToIncrease = int.Parse(code);

            if (numberToIncrease.Equals(99999))
            {
                throw new Exception("cannot generate new document code, threshold reached");
            }

            return docPrefix + ((numberToIncrease + 1).ToString("D5"));

        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString.ToLower();
        }

        public static string GenerateRandomOtp()
        {
            var generator = new Random();
            int codeNumber = generator.Next(10000000, 99999999);
            return codeNumber.ToString("D8");
        }
       
        public static async Task<HttpResponseMessage?> MakeHttpRequest(object requestData, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers = null)
        {
            try
            {

                Uri uri = new Uri(baseAddress);

                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (method == HttpMethod.Post)
                {
                    string data = JsonConvert.SerializeObject(requestData);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    return await client.PostAsync(requestUri, content);
                }
                else if (method == HttpMethod.Get)
                {
                    return await client.GetAsync(requestUri);
                }
                else if (method == HttpMethod.Patch)
                {
                    string data = JsonConvert.SerializeObject(requestData);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    return await client.PatchAsync(requestUri, content);
                }
                else if (method == HttpMethod.Delete)
                {
                    string data = JsonConvert.SerializeObject(requestData);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    return await client.DeleteAsync(requestUri);
                }
                return null;
            }
            catch 
            {
                throw;
            }
        }

        public static string GetRequestIPAddress(HttpRequest httpRequest)
        {

            string ip;
            if (httpRequest.Headers.ContainsKey("X-Forwarded-For"))
                ip = httpRequest.Headers["X-Forwarded-For"];
            else
                ip = httpRequest!.HttpContext!.Connection!.RemoteIpAddress!.MapToIPv4().ToString();

            return ip.ToString();
        }

        public static string GetDeviceInfo(HttpRequest httpRequest)
        {
            return httpRequest.Headers["User-Agent"];
        }

        public static string SanitizeInput(string data)
        {
            data = data.ToString();
            data = data.TrimEnd('/');
            data = data.Trim();
            data = data.RemoveSpecialCharacters();

            return data;

        }

        public static string GenerateSlug(string input)
        {
            string slug = input.ToLowerInvariant();

            StringBuilder validChars = new ();
            foreach (char c in slug)
            {
                if (char.IsLetterOrDigit(c) || c == '-' || c == ' ')
                {
                    validChars.Append(c);
                }
            }
            slug = validChars.ToString();
            slug = slug.Replace(" ", "-");

            return slug;
        }

        public static List<FileImportDto> ParseImportedFileAsync(Stream fileStream)
        {
            List<FileImportDto> logs = new List<FileImportDto>();

            using (TextFieldParser parser = new TextFieldParser(fileStream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields()!;

                    FileImportDto data = new()
                    {
                        ActivityDate = fields[0].ToString(),
                        Description = fields[1].ToString(),
                        Hours = fields[2].ToString(),
                        Minutes = fields[3].ToString(),
                        Seconds = fields[4].ToString(),
                        Property = fields[5].ToString(),
                        TeamMemberEmail = fields[6].ToString(),
                        LogType = fields[7].ToString(),
                        Material = fields[8].ToString(),
                        Activity = fields[9].ToString(),
                        Task = fields[10].ToString(),
                    };

                    logs.Add(data);
                }
            }
            // remvoe the first item which is the headers of the files
            logs.Remove(logs.First());

            return logs;
        }

    }



    public static class StringUtility
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '/' || c == ':' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
