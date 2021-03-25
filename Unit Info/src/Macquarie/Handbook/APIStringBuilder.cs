using System;
using System.Text;

namespace Macquarie.Handbook.WebApi
{
    public abstract class HandbookApiRequestBuilder
    {
        protected StringBuilder API_STRING = new StringBuilder(250);

        private string _code = null;
        public string Code {
            get { return _code; }
            set {
                if (value != null)
                    _code = value;
            }
        }

        protected string BASE_API_STRING { get; set; } = null;
        public int? ImplementationYear { get; set; } = null;
        public int? Limit { get; set; } = null;

        public HandbookApiRequestBuilder() {
            BASE_API_STRING = GetBaseAPIString();
        }

        public HandbookApiRequestBuilder(string code, int? implementationYear) : this() {
            Code = code;
            ImplementationYear = implementationYear;
        }

        protected abstract string GetBaseAPIString();
        protected abstract string GetRequestHeader();

        public override string ToString() {
            if (Code == null || Code.Length == 0)
                return "127.0.0.1";


            API_STRING.Clear();
            API_STRING.Append(BASE_API_STRING);

            if (ImplementationYear != null) {
                API_STRING.Append($"%20+{GetRequestHeader()}.implementationYear:{ImplementationYear}");
            }
            if (Code != null) {
                API_STRING.Append($"%20+{GetRequestHeader()}.code:{Code}");
            }
            if (Limit != null) {
                API_STRING.Append($"/limit/{Limit}");
            }
            return API_STRING.ToString();
        }
        public virtual void Reset() {
            Code = null;
            ImplementationYear = null;
            Limit = null;
        }
    }

    public class UnitApiRequestBuilder : HandbookApiRequestBuilder
    {

        public UnitApiRequestBuilder() : base() { }

        public UnitApiRequestBuilder(string unitCode) : base(unitCode, DateTime.Now.Year) { }

        public UnitApiRequestBuilder(string unitCode, int? implementationYear) : base(unitCode, implementationYear) { }

        public override void Reset() {
            base.Reset();
        }
        protected override string GetRequestHeader() {
            return "mq2_psubject";
        }

        protected override string GetBaseAPIString() {
            //"https://coursehandbook.mq.edu.au/api/content/render/false/query/+contentType:mq2_psubject";
            return "https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_psubject";
        }
    }

    public class CourseApiRequestBuilder : HandbookApiRequestBuilder
    {
        public CourseApiRequestBuilder() : base() { }

        public CourseApiRequestBuilder(string unitCode) : base(unitCode, DateTime.Now.Year) { }

        public CourseApiRequestBuilder(string unitCode, int? implementationYear) : base(unitCode, implementationYear) { }

        public override void Reset() {
            base.Reset();
        }

        protected override string GetBaseAPIString() {
            return "https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_pcourse";
        }

        protected override string GetRequestHeader() {
            return "mq2_pcourse";
        }
    }
}