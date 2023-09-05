using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HttpServerExample
{
    public static class RestUtils
    {
        public static string GetMethod(string requestData)
        {
            string method = String.Empty;
            if (String.IsNullOrEmpty(requestData)) return String.Empty;

            string stringCheck = requestData.Substring(0, requestData.IndexOf(' '));

            method = stringCheck == "GET" ? stringCheck : method;
            method = stringCheck == "POST" ? stringCheck : method;
            method = stringCheck == "PUT" ? stringCheck : method;
            method = stringCheck == "DELETE" ? stringCheck : method;

            return method;
        }

        public static string GetPath(string requestData)
        {
            string path = String.Empty;
            if (String.IsNullOrEmpty(requestData)) return String.Empty;

            path = requestData.Substring(requestData.IndexOf('/'));
            path = path.Substring(0, path.IndexOf(" HTTP"));

            return path;
        }

        public static HttpRequest CreateRequest(string requestData)
        {
            HttpRequest request = new HttpRequest();
            request.Method = GetMethod(requestData);
            request.Url = GetPath(requestData);

            Type reqType = typeof(HttpRequest);
            TypedReference reference = __makeref(request);
            FieldInfo[] reqFields = reqType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            string[] headers = requestData.Split('\n');

            /*int[] remainingIds = new int[headers.Length];
            int remaining = 0;*/

            for (int hindex = 1; hindex < headers.Length; hindex++)
            {
                for (int findex = 2; findex < reqFields.Length; findex++)
                {
                    string fieldName = reqFields[findex].Name;
                    if (headers[hindex].Replace("-", " ").Contains(fieldName))
                    {
                        string value = headers[hindex].Substring(headers[hindex].IndexOf(' '));
                        reqFields[findex].SetValueDirect(reference, value);
                        break;
                    }
                }
            }

            string requestStringRaw = String.Empty;
            for (int i = 0; i < headers.Length; i++)
            {
                requestStringRaw += headers[i] + "\n";
            }

            request.AddOtherHeadres(requestStringRaw);

            return request;
        }
    }

}
