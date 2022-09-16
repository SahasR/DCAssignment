using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublisher
{
    internal class Services
    {
        private int token;
        public string Registration(string userName, string password)
        {
            return "String";
        }

        public int Login(string userName, string password)
        {
            return token;
        }

        public void Publish(string endpoint)
        {
           bool isValid = CheckURLValid(endpoint);

            if(isValid)
            {

            }
        }

        public void Unpublish(string endpoint)
        {
            bool isValid = CheckURLValid(endpoint);

            if (isValid)
            {

            }
        }

        public int GetInt()
        {
            return token;
        }
        private bool CheckURLValid(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
