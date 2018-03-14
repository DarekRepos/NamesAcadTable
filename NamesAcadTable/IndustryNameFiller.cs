using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesAcadTable
{
    public abstract class IndustryNameFiller
    {
        protected string DesignedBy;
        protected string DevelopedBy;
        protected string VerifiedBy;
        protected string GetIndustryName;

        protected IndustryNameFiller AddDesigner(string designer)
        {
            DesignedBy = designer;
            return this;
        }
        protected IndustryNameFiller AddDeveloper(string developer)
        {
            DevelopedBy = developer;
            return this;
        }
        protected IndustryNameFiller AddVerifier(string verification)
        {
            VerifiedBy = verification;
            return this;
        }
        protected IndustryNameFiller AddIndustryName(string name)
        {
            GetIndustryName = name;
            return this;
        }

        public IndustryName FillTable()
        {
            return new IndustryName(DesignedBy, DevelopedBy, VerifiedBy, GetIndustryName);
        }
    }
    public class Electrical : IndustryNameFiller
    {
        public Electrical()
        {
            AddDesigner("Jan Kowalski");
            AddDeveloper("Janina Kowalska");
            AddVerifier("Teodor Przykładowy");
            AddIndustryName("Branża elektryczna");
        }
    }

    public class Sanitary : IndustryNameFiller
    {
        public Sanitary()
        {
            AddDesigner("Jan Rura");
            AddDeveloper("Pola Kowalska");
            AddVerifier("Lilia Kowalska");
            AddIndustryName("Branża sanitarna");
        }
    }

    public class Road : IndustryNameFiller
    {
        public Road()
        {
            AddDesigner("Jack Roads");
            AddDeveloper("Katrine Tree");
            AddVerifier("Bruce Su");
            AddIndustryName("Branża drogowa");
        }
    }
}
