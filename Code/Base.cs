using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;

namespace SeatArrangeAssistant
{
    public class ReadBase
    {
        string dbfn;
        private OleDbConnection conn;
        private static ReadBase s;
        public static ReadBase S
        {
            get
            {
                if (s == null)
                    s = new ReadBase();
                return s;
            }
        }
#pragma warning disable IDE1006 // 命名样式
        public void init(string dbfn)
#pragma warning restore IDE1006 // 命名样式
        {
            this.dbfn = dbfn;
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbfn;
            conn = new OleDbConnection(connstr);
        }
        public DataTable Select(string sql)
        {
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            System.Data.OleDb.OleDbDataAdapter myAd = new OleDbDataAdapter(sql, conn);
            myAd.Fill(dt);
            return dt;
        }
        public bool Execute(string sql)
        {
            bool f = true;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            OleDbCommand sc = new OleDbCommand(sql, conn);
            sc.ExecuteNonQuery();
            return f;
        }

    }

    public class Location
    {
        public int RowX, RowSum;
        public int ColY, ColSum;
        private int Abs;

        public void Setup(Layout lout)
        {
            this.RowSum = lout.Rows;
            this.ColSum = lout.Colums;
        }
        
        public void SetPos(int abs)
        {
            this.Abs = abs;
            CalcRC();
        }

        public void SetPos(int Row, int Col)
        {
            RowX = Row;
            ColY = Col;
        }

        private void CalcRC()
        {
            RowX = Abs / ColSum;
            if (Abs % ColSum != 0) RowX++;
            ColY = Abs % ColSum;
            if (ColY == 0) ColY = ColSum;
        }


        public int GetAbs()
        {
            return (RowX - 1) * ColSum + ColY;
        }

    }
    

    public class Strategy
    {
        public bool IsInit = false;
        public int Probability = 100;
        
        

        public virtual bool Varify() 
        {
            return true;
        }
        

    }

    public class DistanceStrategy : Strategy
    {
        public Student StudentA, StudentB;
        public int Pattern { get; set; }

        public const int Null = -1, Adjacent = 1, Seperate = 0;

        public void Init(Student A, Student B)
        {
            if (A == null || B == null) throw new Exception("Null object");
            StudentA = A;
            StudentB = B;
            IsInit = true;
            Pattern = Null;
        }

        public void Setup(string subType, int probability, Student A, Student B)
        {
            Init(A, B);
            switch (subType)
            {
                case "ADJ":
                    Pattern = Adjacent;
                    break;
                case "SEP":
                    Pattern = Seperate;
                    break;
            }
            Probability = probability;
            

        }

        public double GetDistance()
        {
            double r1 = StudentA.Pos.RowX, r2 = StudentB.Pos.RowX, c1 = StudentA.Pos.ColY, c2 = StudentB.Pos.ColY;
            double result = Math.Sqrt((r1 - r2) * (r1 - r2) + (c1 - c2) * (c1 - c2));
            return result;
        }


        public override bool Varify()
        {
            if (!IsInit) throw new Exception("Strategy short of object");
            if (Pattern == Null) return false;
            if (Pattern == Adjacent) return GetDistance() < 1.01 ? true : false;
            return GetDistance() > 2 ? true : false;
        }

    }

    public class AttributeSet // TODO: fill in attributes
    {
        public int gentle = -1; // -1 -> unknown 0 -> female 1 -> male
        
    }

    public class Student
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public Location Pos { get; set; }
        public Strategy[] Strategies { get; set; }
        public AttributeSet Attribute { get; set; }

        public int SwapCounter
        {
            get => swapCounter;
            set
            {
                swapCounter = value; 

                //TODO: set level
            }
        }

        private int swapCounter = 0;

        public Student(int number, string name, Layout lout)
        {
            Number = number;
            Name = name;
            Pos = new Location();
            Strategies = new Strategy[5];
            Attribute = new AttributeSet();
            Pos.Setup(lout);
        }

        public void SetPosition(int Row, int Col)
        {
            Pos.SetPos(Row, Col);
        }

        public override string ToString()
        {
            return Name + " (" + Number + ") Row: " + Pos.RowX + ", Col: " + Pos.ColY;
        }

        public void SwapPosition(Student p)
        {
            Location temp = Pos;
            Pos = p.Pos;
            p.Pos = temp;
        }
    }

    public class Layout
    {
        public int Rows { get; set; }
        public int Colums { get; set; }
        public bool[,] Filling { get; set; }
        public int Unfilled { get; set; }

        public void Set(int row, int col)
        {
            Rows = row;
            Colums = col;
            Filling = new bool[Rows + 1, Colums + 1];
            Unfilled = Rows * Colums;
        }

        public Layout(int row, int col)
        {
            Set(row, col);
        }

        public void FillAll()
        {
            for (int i = 1; i <= Rows; i++)
            {
                for (int j = 1; j <= Colums; j++)
                {
                    Filling[i, j] = true;
                }
            }
            Unfilled = 0;
        }

        public void Unfill(int row, int col)
        {
            Filling[row, col] = false;
            Unfilled++;
        }
    }

    public class Base
    {
        public Base()
        {
            
        }
        
        public int Read(Student[] arr, Layout lout, Strategy[] strategies)
        {
            //ReadBase
            string apppath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ReadBase.S.init(apppath + "\\class2.mdb");
            string sql = "select * from [class]";
            DataTable dt = ReadBase.S.Select(sql);
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                arr[i] = new Student(Convert.ToInt32(dr["Number"].ToString()) % 100, dr["StudentName"].ToString(), lout);
                arr[i].Pos.Setup(lout);
                arr[i].Pos.SetPos(i);
            }
            int num = i;
            sql = "select * from [Strategy]";
            dt = ReadBase.S.Select(sql);
            i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string type = dr["Type"].ToString();
                switch (type)
                {
                    case "DIS":
                        strategies[i] = new DistanceStrategy();
                        ((DistanceStrategy) strategies[i]).Setup(dr["SubType"].ToString(),
                            Convert.ToInt32(dr["Prob"].ToString()),
                            MainWindow.GetStudent(Convert.ToInt32(dr["Num1"].ToString())),
                            MainWindow.GetStudent(Convert.ToInt32(dr["Num2"].ToString())));
                            //MainWindow.Students[Convert.ToInt32(dr["Num1"].ToString())],
                            //MainWindow.Students[Convert.ToInt32(dr["Num2"].ToString())]);
                        break;
                }
            }
            MainWindow.StrategyNum = i;
            return num;
        }


        private XmlDocument xml = new XmlDocument();
        private XmlElement roomTitle;
        private XmlAttribute rowAttribute;
        private XmlAttribute colAttribute;

        private XmlElement Layout;
        private XmlAttribute EmptyNumAttribute;
        private XmlAttribute[,] EmptyAttributes;
        

        private void initXML()
        {
            XmlDeclaration header = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(header);

            roomTitle = xml.CreateElement("Room");
            Layout = xml.CreateElement("Layout");

            rowAttribute = xml.CreateAttribute("Row");
            colAttribute = xml.CreateAttribute("Col");

            rowAttribute.InnerText = "7";
            colAttribute.InnerText = "6";

            roomTitle.SetAttributeNode(rowAttribute);
            roomTitle.SetAttributeNode(colAttribute);

            xml.AppendChild(roomTitle);

            EmptyNumAttribute = xml.CreateAttribute("EmptyNum");

            EmptyNumAttribute.InnerText = "2";

            EmptyAttributes = new XmlAttribute[3, 3];
            EmptyAttributes[1, 1] = xml.CreateAttribute("R1");
            EmptyAttributes[1, 2] = xml.CreateAttribute("C1");
            EmptyAttributes[2, 1] = xml.CreateAttribute("R2");
            EmptyAttributes[2, 2] = xml.CreateAttribute("C2");
            EmptyAttributes[1, 1].InnerText = EmptyAttributes[2, 1].InnerText = "1";
            EmptyAttributes[1, 2].InnerText = "1";
            EmptyAttributes[2, 2].InnerText = "6";

            Layout.SetAttributeNode(EmptyNumAttribute);
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    Layout.SetAttributeNode(EmptyAttributes[i, j]);
                }
            }
            roomTitle.AppendChild(Layout);
            

            xml.Save("settings.xml");
        }

        public void WriteSettings(Layout lout) //TODO: Test & Debug
        {
            if (!File.Exists("settings.xml"))
            {
                initXML();
            }
            
            roomTitle = (XmlElement)xml.SelectSingleNode("Room");
            rowAttribute = roomTitle.GetAttributeNode("Row");
            colAttribute = roomTitle.GetAttributeNode("Col");

            Layout = (XmlElement)roomTitle.SelectSingleNode("Layout");
            EmptyNumAttribute = Layout.GetAttributeNode("EmptyNum");
            int num = lout.Unfilled;
            EmptyAttributes = new XmlAttribute[num + 1, 3];
            lout.FillAll();

            int count = 0;
            for (int i = 1; i <= lout.Rows; i++)
            {
                for (int j = 1; j <= lout.Colums; j++)
                {
                    if (!lout.Filling[i, j])
                    {
                        count++;
                        EmptyAttributes[count, 1] = Layout.SetAttributeNode("R" + i.ToString(), i.ToString());
                        EmptyAttributes[count, 2] = Layout.SetAttributeNode("C" + i.ToString(), j.ToString());
                    }
                }
            }
            
        }
    

        public Layout LoadSettings()
        {
            Layout lout;
            if (!File.Exists("settings.xml"))
            {
                initXML();
                lout = new Layout(7, 6);
                lout.FillAll();
                lout.Unfill(1, 1);
                lout.Unfill(1, 6);
            }
            else
            {
                xml.Load("settings.xml");
                try
                {
                    roomTitle = (XmlElement)xml.SelectSingleNode("Room");
                    rowAttribute = roomTitle.GetAttributeNode("Row");
                    colAttribute = roomTitle.GetAttributeNode("Col");
                    lout = new Layout(Convert.ToInt32(rowAttribute.InnerText), Convert.ToInt32(colAttribute.InnerText));

                    Layout = (XmlElement) roomTitle.SelectSingleNode("Layout");
                    EmptyNumAttribute = Layout.GetAttributeNode("EmptyNum");
                    int num = Convert.ToInt32(EmptyNumAttribute.InnerText);
                    EmptyAttributes = new XmlAttribute[num + 1 , 3];
                    lout.FillAll();
                    for (int i = 1; i <= num; i++)
                    {
                        EmptyAttributes[i, 1] = Layout.GetAttributeNode("R" + i.ToString());
                        EmptyAttributes[i, 2] = Layout.GetAttributeNode("C" + i.ToString());
                        lout.Unfill(Convert.ToInt32(EmptyAttributes[i, 1].InnerText), Convert.ToInt32(EmptyAttributes[i, 2].InnerText));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fatal errors shown up when loading configuration files: \n" + e.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }

            xml.Save("settings.xml");
            return lout;
        }
    }
}
