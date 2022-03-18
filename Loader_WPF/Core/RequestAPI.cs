using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core
{
    class RequestAPI
    {
		public static string GetToken()
        {
			return "Change";
		}

		public static string GetFile(string filename)
        {
			return String.Format("https://NOTHING/api/files.php?token_auth={0}&file_name={1}", GetToken(), filename);
        }

		public static string GetRequest(string url, string dados)
		{
			HttpWebRequest length = (HttpWebRequest)WebRequest.Create(url);
			
			byte[] bytes = Encoding.UTF8.GetBytes(dados);
			length.Method = "POST";
			length.Headers["X-Auth-Loader"] = GetToken();
			length.ContentType = "application/x-www-form-urlencoded";
			length.ContentLength = (long)((int)bytes.Length);

			using (Stream requestStream = length.GetRequestStream())
			{
				requestStream.Write(bytes, 0, (int)bytes.Length);
			}

			WebResponse response = (HttpWebResponse)length.GetResponse();
			return (new StreamReader(response.GetResponseStream())).ReadToEnd();
		}
	}
}
