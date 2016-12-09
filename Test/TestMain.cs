using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class TestMain : Form
    {
        public TestMain()
        {
            InitializeComponent();          
        }

        private int temperature;
        public string type = "RealFire 001";       // 添加型号作为演示
        public string area = "China Xian";         // 添加产地作为演示

        //新建一个BoiledEventArgs类继承自EventArgs
        public class BoiledEventArgs : EventArgs
        {
            public readonly int temperature;
            public BoiledEventArgs(int _temperature)//为自定义BoiledEventArgs类构建一个带有参数名为“_temperature”的实例构造函数
            {
                this.temperature = _temperature;
            }
        }

        public delegate void BoiledEventHandler(object sender, BoiledEventArgs e);//声明委托
        public event BoiledEventHandler BoiledEvent; //声明事件

        // 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        protected virtual void OnBoiled(BoiledEventArgs e)
        {
            BoiledEvent(this, e);  // 调用所有注册对象的方法           
        }

        // 烧水。
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;
                if (temperature > 95)
                {
                    //建立BoiledEventArgs 对象。
                    BoiledEventArgs e = new BoiledEventArgs(temperature);
                    OnBoiled(e);  // 调用 OnBolied方法
                }
            }
        }

        // 警报器       
           public void MakeAlert(object sender, BoiledEventArgs e)
            {             
                Console.WriteLine("Alarm：{0} - {1}: ", area, type);
                Console.WriteLine("Alarm: 嘀嘀嘀，水已经 {0} 度了：", e.temperature);
                Console.WriteLine();
            }

        // 显示器
            public void ShowMsg(object sender, BoiledEventArgs e)
            {                
                Console.WriteLine("Display：{0} - {1}: ",area,type);
                Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", e.temperature);
                Console.WriteLine();
            }
        

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() == String.Empty)//当textBox1文本框的内容为空时发生
            {
                errorProvider1.SetError((TextBox)sender, "这里不能为空！");
                e.Cancel = true;//控件值无效，取消事件继续执行，焦点仍停留在当前控件中
            }
            else
            {
                errorProvider1.SetError((TextBox)sender, "");//通过验证则不显示任何错误提示字符
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CompleteCombobox cc = new CompleteCombobox();
            cc.Show();
        }
        /// <summary>
        /// 清除容器里面某些控件的值
        /// </summary>
        /// <param name="parContainer">容器类控件</param>

        private void button2_Click(object sender, EventArgs e)
        {
            ClearCntrValue(this);
            
        }
        public void ClearCntrValue(Control parContainer)
        {
            for (int index = 0; index < parContainer.Controls.Count; index++)
            {
                // 如果是容器类控件，递归调用自己
                if (parContainer.Controls[index].HasChildren)
                {
                    ClearCntrValue(parContainer.Controls[index]);
                }
                else
                {
                    switch (parContainer.Controls[index].GetType().Name)
                    {
                        case "TextBox":
                            parContainer.Controls[index].Text = "";
                            break;
                        case "RadioButton":
                            ((RadioButton)(parContainer.Controls[index])).Checked = false;
                            break;
                        case "CheckBox":
                            ((CheckBox)(parContainer.Controls[index])).Checked = false;
                            break;
                        case "ComboBox":
                            ((ComboBox)(parContainer.Controls[index])).Text = "";
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 针对大量 RadioButton控件选项条件的取值筛选
        /// 把所有RadioButton控件的CheckedChanged事件用同一个事件替换,如下：
        /// this.radioButton2.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChanged);
        /// </summary>
        /// <param name="parContainer">容器类控件</param>
        private void RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;

            string filterValue = string.Empty;
            switch (((RadioButton)sender).Tag.ToString())
            {
                case "All":
                    filterValue = "ALL";
                    break;              
                case "Inbound":
                    filterValue = "Inbound";
                    break;
                case "UnBill":
                    filterValue = "Outbound";
                    break;
            }
            //this.gvData.DataSource = GetDataSource(filterValue);

        }

        private void TestMain_Load(object sender, EventArgs e)
        {
            // 以下为委托和事件的使用
            BoiledEvent += MakeAlert;
            BoiledEvent += ShowMsg;
            BoilWater();
         }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                WaitFormService.CreateWaitForm();
                Assembly asmb = Assembly.GetExecutingAssembly();
                Object obj = asmb.CreateInstance(WaitFormService.Instance.ToString());
                Form frm = obj as Form;
                this.Show(frm);
                WaitFormService.CloseWaitForm();
            }
            catch (Exception ex)
            {
                WaitFormService.CloseWaitForm();
            }
        }

        /// <summary>
        /// 测试委托、事件
        /// 实现范例的Observer(观察者)设计模式       
        /// </summary>                  

    }
}
