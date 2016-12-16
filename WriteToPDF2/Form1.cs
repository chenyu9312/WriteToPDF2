using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using System.Diagnostics;

namespace WriteToPDF2
{
    public partial class Form1 : Form
    {
        //variables
        string pathin = "";
        string pathout = "";
        int pageNumber = 0;
        string range = "";
        int startPage = 0;
        int endnotePage = 0;
        public Form1()
        {
            InitializeComponent();
            
        }
         
private void button1_Click(object sender, EventArgs e)
        {
           
            //get upload file path directory and set up new file location
            pathin = openFileDialog1.FileName;
            

            pathout = Path.GetDirectoryName(pathin) + "/SOP Short Picking Ticket Form Note.pdf";
            
            try
            {
               
           
                //create a document object
                var doc = new Document(PageSize.A4);

                //create PdfReader object to read from the existing document
                PdfReader reader = new PdfReader(pathin);
                pageNumber = reader.NumberOfPages;
                range = "1-" + pageNumber.ToString();
                if (String.IsNullOrEmpty(endTextBox.Text))
                {
                    startPage = 1;
                    endnotePage = pageNumber;
                }
                else
                {
                    startPage = Convert.ToInt32(startTextBox.Text);
                    endnotePage = Convert.ToInt32(endTextBox.Text);
                }
                //select  pages from the original document
                reader.SelectPages(range);

                //create PdfStamper object to write to get the pages from reader 
                PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout, FileMode.Create));
                // PdfContentByte from stamper to add content to the pages over the original content
                
                for (int i = startPage;  i <= endnotePage; i++)
                {
                    PdfContentByte pbover = stamper.GetOverContent(i);
                    //add content to the page using ColumnText
                    ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(textBox1.Text), 100, 400, 0);
                    // PdfContentByte from stamper to add content to the pages under the original content
                    PdfContentByte pbunder = stamper.GetUnderContent(pageNumber);
                }
                
               
            
                //close the stamper
                stamper.Close();
                //close the reader
                reader.Close();
                //clean up
                doc.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("please enter an interger");
            }
            MessageBox.Show("note added to the pdf");
            Process.Start(pathout);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            
            openFileDialog1.Filter = "PDF Files|*.pdf";
            openFileDialog1.Title = "Select a PDF File";
           
            // If the user clicked OK in the dialog and
            // a .pdf file was selected, open it.
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                uploadFileTextBox.Text = openFileDialog1.SafeFileName;
                pathin =openFileDialog1.FileName;
            
            }
            

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Process.Start(pathout);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
