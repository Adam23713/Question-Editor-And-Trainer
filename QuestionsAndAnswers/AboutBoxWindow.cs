using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionsAndAnswers
{
    partial class AboutBoxWindow : Form
    {
        public AboutBoxWindow()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = "Questions Editor";
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = "Source Code: " + AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
            this.textBoxDescription.Text += GetLicense();
            this.webPageLabel.Text = "Web Page: " + "https://callprog.hu/";
        }

        private string GetLicense()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("BSD 3-Clause License");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("Copyright (c) 2020, Kertész Ádám");
            builder.Append(Environment.NewLine);
            builder.Append("All rights reserved.");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("Redistribution and use in source and binary forms, with or without");
            builder.Append(Environment.NewLine);
            builder.Append("modification, are permitted provided that the following conditions are met:");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("1. Redistributions of source code must retain the above copyright notice, this");
            builder.Append(Environment.NewLine);
            builder.Append("list of conditions and the following disclaimer.");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("2. Redistributions in binary form must reproduce the above copyright notice,");
            builder.Append(Environment.NewLine);
            builder.Append("this list of conditions and the following disclaimer in the documentation");
            builder.Append(Environment.NewLine);
            builder.Append("and/or other materials provided with the distribution.");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("3. Neither the name of the copyright holder nor the names of its");
            builder.Append(Environment.NewLine);
            builder.Append("contributors may be used to endorse or promote products derived from");
            builder.Append(Environment.NewLine);
            builder.Append("this software without specific prior written permission.");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\"");
            builder.Append(Environment.NewLine);
            builder.Append("AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE");
            builder.Append(Environment.NewLine);
            builder.Append("IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE");
            builder.Append(Environment.NewLine);
            builder.Append("DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE");
            builder.Append(Environment.NewLine);
            builder.Append("FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL");
            builder.Append(Environment.NewLine);
            builder.Append("DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR");
            builder.Append(Environment.NewLine);
            builder.Append("SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER");
            builder.Append(Environment.NewLine);
            builder.Append("CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,");
            builder.Append(Environment.NewLine);
            builder.Append("OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE");
            builder.Append(Environment.NewLine);
            builder.Append("OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.");
            builder.Append(Environment.NewLine);

            return builder.ToString();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
