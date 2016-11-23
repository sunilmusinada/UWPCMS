using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Template
{
    internal class Constants
    {
        public static readonly string TaskName = "SyncSurveyTask";
        public static readonly string SurveyFolder = "Surveys";
        public static readonly string TempSurveyFolder = "TempSurveys";
        public const string InProgressStatus = "In Progress";
        public const string PendingReview = "Pending Review";
        public const string Submitted = "Submitted";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public static readonly string OtherSurveysFolder = "OtherUsers";
        public static readonly String HTML_CONTROL_LABEL = "label";
	public static readonly String HTML_CONTROL_RADIO = "radio";
	public static readonly String HTML_CONTROL_CHECK_BOX = "checkbox";
	public static readonly String HTML_CONTROL_SELECT = "select";
	public static readonly String HTML_CONTROL_TEXT_BOX = "textbox";
	public static readonly String HTML_CONTROL_DATE = "date";
	public static readonly String HTML_CONTROL_NUMBER = "number";
	public static readonly String HTML_CONTROL_TEXT_AREA = "textarea";
	public static readonly String HTML_CONTROL_FILE_UPLOAD = "file";
    public static List<int> INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS = new List<int>();
    public static List<int> INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_RENDER_TRUE = new List<int>();
        public static List<int> LIST_4A3_TO_4A6 = new List<int> {14500,
       14600,
        14700,
        14800};
        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4A3 = 14400;
        

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B2 = 15700;
        public static List<int> LIST_4B2_TO_4B7 = new List<int> { 15800,
        15900,
		16000,
		16100,
		16200,
		16300};


        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3A5 = 10100;
        public static List<int> LIST_3A6_TO_3A18 = new List<int> {10300,
       10400,
		10500,
		10600,
		10700,
		10800,
		10900,
		11000,
		11100,
		11200,
		11300,
		11400,
		11500,
		11600};


        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B9 = 16500;
        public static List<int> LIST_4B9_TO_413 = new List<int> { 16600,
        16700,
		16800,
		16900,
		17000};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4C1 = 17300;
        public static List<int> LIST_4C1_TO_4C8 = new List<int> { 17400,
        17500,
		17600,
		17700,
		17800,
		17900,
		18000,
		18100,
		18200};


        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4C9 = 18400;
        public static List<int> LIST_4C9_TO_4C15 = new List<int> { 18500,
       18600,
		18700,
		18800,
		18900,
		19000,
		19100 };


        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4D1 = 19300;
        public static List<int> LIST_4D1_TO_4D3 = new List<int> {19400,
       19500,
        19600};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I9 = 24400;
        public static List<int> LIST_4I9_TO_4I17 = new List<int> {24500,
       24600,
		24700,
		24800,
		24900,
		25000,
		25100,
		25200,
		25300};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3B5 = 12000;
        public static List<int> LIST_3B5_TO_3B19 = new List<int> { 12100,
        12150,
		12200,
		12300,
		12400,
		12500,
		12600,
		12700,
		12800,
		12900, 
		13000,
		13100,
		13200,
		13300,
		13400,
		13500,
		13600,
		13700};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4G2 = 21650;
        public static List<int> LIST_4G2_TO_4G9 = new List<int> { 21700,
        21800,
		21900,
		22000,
		22100,
		22200,
		22300,
		22400};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4H2 = 22650;
        public static List<int> LIST_4H2_TO_4H8 = new List<int> { 22700,
        22800,
		22900,
		23000,
		23100,
		23200,
		23300 };

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I1 = 23450;
        public static List<int> LIST_4I1_TO_4I8 = new List<int> { 23500,
        23600,
		23700,
		23800,
		23900,
		24000,
		24100,
		24200,};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3B_SECTION = 11560;
        public static List<int> LIST_3B1_TO_3B19 = new List<int> { 11600,
        11700,
		11800,
		11900,
		12000,
		12100,
		12150,
		12200,
		12300,
		12400,
		12500,
		12600,
		12700,
		12800,
		12900,
		13000,
		13100,
		13200,
		13300,
		13400,
		13500,
		13600,
		13700};


        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4A_SECTION = 14050;
        public static List<int> LIST_4A1_TO_4A12 = new List<int> { 14100,
        14200,
		14300,
		14400,
		14500,
		14600,
		14700,
		14800,
		14900,
		15000,
		15100,
		15100,
		15200,
		15300,
		15400};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B_SECTION = 15450;
        public static List<int> LIST_4B1_TO_4B14 = new List<int> { 15500,
        15600,
		15700,
		15800,
		15900,
		16000,
		16100,
		16200,
		16300,
		16400,
		16500,
		16600,
		16700,
		16800,
		16900,
		17000,
		17100
    };

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4F_SECTION = 20150;
        public static List<int> LIST_4F1_TO_4F12 = new List<int> { 20200,
        20250,
		20300,
		20400,
		20500,
		20600,
		20700,
		20800,
		20900,
		21000,
		21100,
		21200,
		21300,
		21400};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4G_SECTION = 21450;
        public static List<int> LIST_4G1_TO_4G9 = new List<int> { 21500,
        21600,
		21650,
		21700,
		21800,
		21900,
		22000,
		22100,
		22200,
		22300,
		22400};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4H_SECTION = 22450;
        public static List<int> LIST_4H1_TO_4H8 = new List<int> { 22500,
        22600,
		22650,
		22700,
		22700,
		22800,
		22900,
		23000,
		23100,
		23200,
		23300};

        public static int INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I_SECTION = 23350;
        public static List<int> LIST_4I1_TO_4I17 = new List<int> { 23400,
        23450,
		23500,
		23600,
		23700,
		23800,
		23900,
		24000,
		24100,
		24200,
		24300,
		24400,
		24500,
		24600,
		24700,
		24800,
		24900,
		25000,
		25100,
		25200,
		25300};
        public static Dictionary<int, List<int>> QuestionSectionDictionary = new Dictionary<int, List<int>> { { INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4A3,LIST_4A3_TO_4A6},
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B2,LIST_4B2_TO_4B7 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3A5,LIST_3A6_TO_3A18 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B9,LIST_4B9_TO_413 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4C1,LIST_4C1_TO_4C8 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4D1,LIST_4D1_TO_4D3 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I9,LIST_4I9_TO_4I17 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3B5,LIST_3B5_TO_3B19 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4G2,LIST_4G2_TO_4G9 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4H2,LIST_4H2_TO_4H8 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I1,LIST_4I1_TO_4I8 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_3B_SECTION,LIST_3B1_TO_3B19 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4A_SECTION,LIST_4A1_TO_4A12 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4B_SECTION,LIST_4B1_TO_4B14 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4F_SECTION,LIST_4F1_TO_4F12 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4G_SECTION,LIST_4G1_TO_4G9 },
            { INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4H_SECTION,LIST_4H1_TO_4H8 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4I_SECTION,LIST_4I1_TO_4I17 },
            {INFECTION_CONTROL_SKIP_LOGIC_QUESTIONS_ID_4C9,LIST_4C9_TO_4C15}
        };

        public static Dictionary<int, string> CheckBoxAnswerDictionary = new Dictionary<int, string> { { 88600, "Yes - Stop here and skip to section 3.B" },
            {101610,"Section 3.B. is Not Applicable to this survey." },
            {104700,"Yes - Stop here and skip to section 3.C" },
            {121750,"Section 4.A. is Not Applicable to this survey." },
            {123710,"No observations available (If selected, ALL questions from 4.A.3 – 4.A.6 will be blocked)" },
            {133750,"Section 4.B. is Not Applicable to this survey." },
            {134710,"No observations available (If selected, ALL questions from 4.B.2 – 4.B.7 will be blocked)" },
            {141710,"No observations available (If selected, ALL questions from 4.B.9 – 4.B.13 will be blocked)" },
            {147710,"No observations available (If selected, ALL questions from 4.C.1 – 4.C.8 will be blocked)" },
            {155710," No observations available (If selected, ALL questions from 4.C.9 – 4.C.15 will be blocked)" },
            {162800,"No observation available (if selected ALL questions from 4.D.1-4.D.3 will be blocked)" },
            {169950,"Section 4.F. is Not Applicable to this survey." },
            {181950,"Section 4.G. is Not Applicable to this survey." },
            {182950,"No observation available (If selected, ALL questions from 4.G.2-4.G.9 will be blocked)" },
            {190950,"Section 4.H. is Not Applicable to this survey." },
            {191950,"No observation available (If selected, ALL questions from 4.H.2-4.H.8 will be blocked)" },
            {198940,"Section 4.I. is Not Applicable to this survey." },
            {198950,"No observation available (If selected, ALL questions from 4.I.1-4.I.8 will be blocked)" },
            {206900, "No - Questions 4.I.9 – 4.I.17 will be blocked"}
        };

    }
}
