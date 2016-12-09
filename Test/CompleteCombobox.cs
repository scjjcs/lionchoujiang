using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class CompleteCombobox : Form
    {
        public CompleteCombobox()
        {
            InitializeComponent();
        }
        //初始化绑定默认关键词（此数据源可以从数据库取）

        List<string> listOnit = new List<string>();

        //输入key之后，返回的关键词

        List<string> listNew = new List<string>();


        private void CompleteCombobox_Load(object sender, EventArgs e)

        {

            //调用绑定

            BindComboBox();

        }

        /// <summary>

        /// 绑定ComboBox

        /// </summary>

        private void BindComboBox()

        {

            listOnit.Add("张三");

            listOnit.Add("张思");

            listOnit.Add("张五");

            listOnit.Add("王五");

            listOnit.Add("刘宇");

            listOnit.Add("马六");

            listOnit.Add("孙楠");

            listOnit.Add("那英");

            listOnit.Add("刘欢");



            /*

             * 1.注意用Item.Add(obj)或者Item.AddRange(obj)方式添加

             * 2.如果用DataSource绑定，后面再进行绑定是不行的，即便是Add或者Clear也不行

             */

            this.comboBox1.Items.AddRange(listOnit.ToArray());

        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)

        {

            //清空combobox

            this.comboBox1.Items.Clear();

            //清空listNew

            listNew.Clear();

            //遍历全部备查数据

            foreach (var item in listOnit)

            {

                if (item.Contains(this.comboBox1.Text))

                {

                    //符合，插入ListNew

                    listNew.Add(item);

                }

            }

            //combobox添加已经查到的关键词

            this.comboBox1.Items.AddRange(listNew.ToArray());

            //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列

            this.comboBox1.SelectionStart = this.comboBox1.Text.Length;

            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。

            Cursor = Cursors.Default;

            //自动弹出下拉框

            this.comboBox1.DroppedDown = true;

        }
    }
}
