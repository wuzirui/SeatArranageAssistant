using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeatArrangeAssistant
{
    /// <summary>
    /// seat.xaml 的交互逻辑
    /// </summary>
    public partial class seat : UserControl
    {
        private Student relateStudent;

        public bool Choosen = false;
        public bool isHighLight = false;
        public bool IsNull = false;
        public static int NumChoosen = 0;
        public static seat A, B;
        public int Row, Col;
        public bool isLocked = false;

        public void Lock()
        {
            isLocked = true;
            Highlight();
        }

        public void Unlock()
        {
            isLocked = false;
            CancelHighLight();
        }

        public Student RelateStudent
        {
            get => relateStudent;
            set
            {
                relateStudent = value;
                if (value != null)
                    relateStudent.SetPosition(Row, Col);
            }
        }

        public seat()
        {
            InitializeComponent();
        }

        public void settle(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void Setup(Student Relavant)
        {
            if (Relavant == null)
            {
                IsNull = true;
                RelateStudent = null;
                body.Content = "null";
                return;
            }
            RelateStudent = Relavant;
            body.Content = Relavant.Name;
            IsNull = false;
            body.ToolTip = RelateStudent.ToString();
        }

        public void swap(seat q)
        {
            Student temp = RelateStudent;
            Setup(q.RelateStudent);
            q.Setup(temp);
            if (q.isHighLight && !isHighLight)
            {
                q.CancelHighLight();
                Highlight();
            }
            else if(!q.isHighLight && isHighLight)
            {
                q.Highlight();
                CancelHighLight();
            }
            if (q.Choosen)
            {
                if (Choosen)
                {
                    q.Shut();
                    Shut();

                    q.Open();
                    Open();
                    return;
                }
                q.Shut();
                Open();
                return;
            }

            if (Choosen)
            {
                q.Open();
                Shut();
            }
            RelateStudent.SetPosition(Row, Col);
            q.RelateStudent.SetPosition(q.Row, q.Col);
        }

        public void Highlight()
        {
            body.Background = SystemColors.DesktopBrush;
            isHighLight = true;
        }

        public void CancelHighLight()
        {
            body.Background = SystemColors.MenuHighlightBrush;
            isHighLight = false;
            if (Choosen)
                body.Background = SystemColors.ActiveCaptionBrush;
        }

        public void swap(seat q, bool f)
        {
            Student temp = RelateStudent;
            Setup(q.RelateStudent);
            q.Setup(temp);
            if (q.isHighLight && !isHighLight)
            {
                
            }
            if (q.Choosen)
            {
                if (Choosen) return;
                q.Shut();
                Open();
                return;
            }
            if (Choosen)
            {
                q.Open();
                Shut();
            }

        }

        public void Shut()
        {
            body.Background = SystemColors.MenuHighlightBrush;
            Choosen = false;
            NumChoosen--;
            if (A == this) A = null;
            else B = null;
        }

        public void Open()
        {
            body.Background = SystemColors.ActiveCaptionBrush;
            Choosen = true;
            NumChoosen++;
            if (A == null) A = this;
            else B = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(RelateStudent.ToString());
            if (Choosen)
            {
                Shut();
            }
            else
            {
                if (NumChoosen == 2)
                    return;
                Open();
            }
        }

        public const int R = 0;
        public const int SR = 1;
        public const int SSR = 2;
        public void setLevel(int level)
        {
            string[] str = {"R", "SR", "SSR"};

            Brush[] br =
            {
                new SolidColorBrush(Color.FromRgb(128, 138, 135)), new SolidColorBrush(Color.FromRgb(255, 155, 0)),
                new SolidColorBrush(Color.FromRgb(255, 69, 0))
            };
            TAG.Text = str[level];
            TAG.Foreground = br[level];
        }

        public const int RVal = 0, SRVal = 6, SSRVal = 8;

        public void CheckLevel(int counterVal)
        {
            if (counterVal >= RVal) setLevel(R);
            if (counterVal >= SRVal) setLevel(SR);
            if (counterVal >= SSRVal) setLevel(SSR);
        } 

    }
}
