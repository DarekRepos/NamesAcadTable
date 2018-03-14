using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesAcadTable
{
    public class IndustryName
    {
        private readonly string _designedBy;
        private readonly string _developedBy;
        private readonly string _verifiedBy;
        private readonly string _industryName;

        public string DesignedBy => _designedBy;
        public string DevelopedBy => _developedBy;
        public string VerifiedBy => _verifiedBy;
        public string GetIndustryName => _industryName;

        public IndustryName(string designer, string developer, string verifier, string name)
        {
            _designedBy = designer;
            _developedBy = developer;
            _verifiedBy = verifier;
            _industryName = name;
        }

    }
}
