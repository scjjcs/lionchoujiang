using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Test
{
    class WaitFormService
    {
        public static void CreateWaitForm()
        {
            WaitFormService.Instance.CreateForm();
        }

        public static void CloseWaitForm()
        {
            WaitFormService.Instance.CloseForm();
        }

        public static void SetWaitFormCaption(string text)
        {
            WaitFormService.Instance.SetFormCaption(text);
        }

        private static WaitFormService _instance;
        private static readonly Object syncLock = new Object();

        public static WaitFormService Instance
        {
            get
            {
                if (WaitFormService._instance == null)
                {
                    lock (syncLock)
                    {
                        if (WaitFormService._instance == null)
                        {
                            WaitFormService._instance = new WaitFormService();
                        }
                    }
                }
                return WaitFormService._instance;
            }
        }

        private WaitFormService()
        {
        }

        private Thread waitThread;
        private WaitForm waitForm;

        public void CreateForm()
        {
            if (waitThread != null)
            {
                try
                {
                    waitForm.Dispose();
                    waitThread.Abort();
                    waitThread.Join();
                    waitThread.DisableComObjectEagerCleanup();
                }
                catch (Exception)
                {
                }
            }

            waitThread = new Thread(new ThreadStart(delegate ()
            {
                waitForm = new WaitForm();
                Application.Run(waitForm);
            }));
            waitThread.Start();
        }

        public void CloseForm()
        {
            if (waitThread != null)
            {
                try
                {
                    waitForm.Dispose();
                    waitThread.Abort();
                    waitThread.Join();
                    waitThread.DisableComObjectEagerCleanup();
                }
                catch (Exception)
                {
                }
            }
        }

        public void SetFormCaption(string text)
        {
            if (waitForm != null)
            {
                try
                {
                    waitForm.SetText(text);
                }
                catch (Exception)
                {
                }
            }
        }

    }
}
