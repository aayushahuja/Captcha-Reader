using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;

namespace Captcha
{
    public partial class Form1 : Form
    {
        Stack currentColour;
        //ArrayList segments;
        //Color currentColour;
        Bitmap imgBitmap;
        Image file;
        public Form1()
        {
            InitializeComponent();
        }
        private static Bitmap crop(Bitmap img, int[] arr)
        {
            Bitmap croppedImage = new Bitmap(arr[2] - arr[0] + 1, arr[3] - arr[1] + 1);
            for (int x = arr[0]; x <= arr[2]; x++)
            {
                for (int y = arr[1]; y <= arr[3]; y++)
                {
                    croppedImage.SetPixel(x - arr[0], y - arr[1], img.GetPixel(x, y));
                }
            }
            return croppedImage;
        }
        private static Array extremes(Bitmap imgBitmap)
        {
            //finding xTopLeft
            int xTopLeft = 0;
            Boolean found = false;
            for (int x = 0; x < imgBitmap.Width; x++)
            {
                for (int y = 0; y < imgBitmap.Height; y++)
                {
                    Color colour = imgBitmap.GetPixel(x, y);
                    if (colour.GetBrightness() < .5)
                    {
                        found = true;
                        break;
                    }

                }
                if (found == true)
                {
                    break;
                }
                else
                {
                    xTopLeft++;
                }
            }

            //finding yTopLeft
            found = false;
            int yTopLeft = 0;
            for (int y = 0; y < imgBitmap.Height; y++)
            {
                for (int x = 0; x < imgBitmap.Width; x++)
                {
                    Color colour = imgBitmap.GetPixel(x, y);
                    if (colour.GetBrightness() < .5)
                    {
                        found = true;
                        break;
                    }

                }
                if (found == true)
                {
                    break;
                }
                else
                {
                    yTopLeft++;
                }
            }

            //finding xBottomRight
            int xBottomRight = imgBitmap.Width - 1;
            found = false;
            for (int x = imgBitmap.Width - 1; x >= 0; x--)
            {
                for (int y = imgBitmap.Height - 1; y >= 0; y--)
                {
                    Color colour = imgBitmap.GetPixel(x, y);
                    if (colour.GetBrightness() < .5)
                    {
                        found = true;
                        break;
                    }

                }
                if (found == true)
                {
                    break;
                }
                else
                {
                    xBottomRight--;
                }
            }
            //finding yBottomRight
            int yBottomRight = imgBitmap.Height - 1;
            found = false;
            for (int y = imgBitmap.Height - 1; y >= 0; y--)
            {
                for (int x = imgBitmap.Width - 1; x >= 0; x--)
                {
                    Color colour = imgBitmap.GetPixel(x, y);
                    if (colour.GetBrightness() < .5)
                    {
                        found = true;
                        break;
                    }

                }
                if (found == true)
                {
                    break;
                }
                else
                {
                    yBottomRight--;
                }
            }
            int[] arr = new int[4] ;
            arr[0] = xTopLeft;
            arr[1] = yTopLeft;
            arr[2] = xBottomRight;
            arr[3] = yBottomRight;
            return arr;
        }
        
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                imgBitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
            }
        }

        private static Bitmap makeOthersWhite(Bitmap img,Color colour){
            Bitmap a = new Bitmap(img);
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    if (img.GetPixel(x, y) != colour)
                    {
                        a.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                }
            }
                return a;
        }
        private static Bitmap crop(int startIndex, int endIndex,Bitmap img)
        {
            
            Bitmap a = new Bitmap(endIndex - startIndex + 1, img.Height);
            for (int x = startIndex; x <= endIndex; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    a.SetPixel(x - startIndex,y,img.GetPixel(x, y));
                }
            }
            return a;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            
            //currentColour = Color.FromArgb(255,255,255);
            currentColour = new Stack();
            //segments.Add(imgBitmap);
               
            //removing noise
            for (int x = 0; x < imgBitmap.Width; x++)
            {
                for (int y = 0; y < imgBitmap.Height; y++)
                {
                    Color colour = imgBitmap.GetPixel(x, y);
                    if (colour.GetBrightness() > .5)
                    {
                        imgBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        imgBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                   
                }
            }
            pictureBox2.Image = imgBitmap;
            checkBox1.Checked = true;

            //narowing down the search
            int[] croppedImgExtremes = (int[])extremes(imgBitmap);
            //got (xtopleft,yTopLeft) and (xBottomRight,yBottomRight)
            Bitmap croppedImage = crop(imgBitmap, croppedImgExtremes);
            //got cropped image
            pictureBox3.Image = croppedImage;

            //removing white regions
            ArrayList onlyWhite = new ArrayList();
            ArrayList endPoints = new ArrayList();
            bool flag = false;
            for (int x = 0; x < croppedImage.Width; x++)
            {
                flag = true;
                for (int y = 0; y < croppedImage.Height; y++)
                {
                    if (croppedImage.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                    {
                        flag = false;
                    }
                }
                if (flag == true)
                {
                    onlyWhite.Add(x);
                    
                    //adding one extra line
                    if (onlyWhite.Count > 1)
                    {
                        if ((int)onlyWhite[onlyWhite.Count - 1] != (int)((int)onlyWhite[onlyWhite.Count - 2] + 1))
                        {
                            endPoints.Add(onlyWhite[onlyWhite.Count - 2]);
                            endPoints.Add(onlyWhite[onlyWhite.Count - 1]);

                        }
                    }
                }
            }
            endPoints.Insert(0, onlyWhite[0]);
            endPoints.Add(onlyWhite[onlyWhite.Count - 1]);
            
            /*
            for (int i = 1; i < onlyWhite.Count; i++)
            {
                if ((int)onlyWhite[i] == ((int)onlyWhite[(i - 1)] + 1)) { continue; }
                else { endPoints.Add(onlyWhite[i-1]); onlyWhite.RemoveAt(i - 1); i--; }
            }
            */
            int newWidth = croppedImage.Width - onlyWhite.Count;

            //cropping again
            Bitmap croppedImage2 = new Bitmap(newWidth, croppedImage.Height);
            int x2=0;
            for (int x = 0; x < croppedImage.Width;x++ )
            {
                if (onlyWhite.Contains(x))
                {
                    continue;
                }
                else
                {
                    for (int y = 0; y < croppedImage2.Height; y++)
                    {
                        croppedImage2.SetPixel(x2, y, croppedImage.GetPixel(x, y));
                    }
                    x2++;
                }
            }
            pictureBox8.Image = croppedImage2;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            textBox1.Text = ((int)endPoints[0]).ToString();
            textBox2.Text = endPoints[1].ToString();
           // textBox3.Text = endPoints[2].ToString();
            Bitmap dummy;
            dummy=crop(0, (int)(endPoints[0]), croppedImage);
            dummy = crop(dummy, (int[])extremes(dummy));
            pictureBox4.Image = dummy;

            if (endPoints.Count > 2)
            {
                dummy=crop((int)endPoints[1], (int)endPoints[2], croppedImage);
                dummy = crop(dummy, (int[])extremes(dummy));
                pictureBox5.Image = dummy;
                if (endPoints.Count <= 4)
                {
                    dummy = crop((int)endPoints[3], ((int)(croppedImage.Width) - 1), croppedImage);
                    dummy = crop(dummy, (int[])extremes(dummy));
                    pictureBox6.Image = dummy;
                }
                else
                {
                    dummy = crop((int)endPoints[3], (int)endPoints[4], croppedImage);
                    dummy = crop(dummy, (int[])extremes(dummy));
                    pictureBox6.Image = dummy;
                    dummy = crop((int)endPoints[5], ((int)(croppedImage.Width) - 1), croppedImage);
                    dummy = crop(dummy, (int[])extremes(dummy));
                    pictureBox7.Image = dummy;
                }
            }
            else
            {
                dummy=crop((int)endPoints[1], ((int)(croppedImage.Width) - 1), croppedImage);
                dummy = crop(dummy, (int[])extremes(dummy));
                pictureBox5.Image = dummy;
            }
            
            /*
            int end1 = segmentXIndex(croppedImage2, 0);
            int end2 = segmentXIndex(croppedImage2, end1);
            if(end2<croppedImage2.Width){
                int end3 = segmentXIndex(croppedImage2,end2);
            }*/
            /*Color[] doneColours = new Color[5];
            int countDoneColours = 0;
            Bitmap a;
            Bitmap[] segments = new Bitmap[5];
            bool contain = false;
            int[] b;
            doneColours[0] = Color.FromArgb(23, 23, 23);
            Color ac = Color.FromArgb(23, 23, 23);
            Color colours;
            /*if (doneColours[0] == ac)
            {
                textBox1.Text = croppedImage.GetPixel(23,23);
            }*/
                //segmenting parts take one colour make others zero then distance between extremes do
            /*   for (int x = 0; x < croppedImage.Width; x++)
                {
                    for (int y = 0; y < croppedImage.Height; y++)
                    {
                        colours = croppedImage.GetPixel(x, y);
                        contain = false;
                        for (int i = 0; i < countDoneColours; i++)
                        {
                            if (doneColours[i]==colours)
                            {
                              //  Environment.Exit(0);
                                contain = true;
                                break;
                            }
                        }
                            if ((colours.GetBrightness() < .5) && contain==false)
                            {
                                a = makeOthersWhite(croppedImage, colours);
                                b = (int[])extremes(a);
                                pictureBox4.Image = crop(croppedImage, b);
                                //Environment.Exit(0);
                                /*if ((b[2] - b[0]) > ((croppedImage.Width) / 2.8))
                                {

                                }
                                else {
                                    segments.Add(crop(croppedImage, b));

                                }*/
            
              /*                  textBox1.Text = countDoneColours.ToString();
                                pictureBox4.Image = crop(croppedImage, b);
                              //  segments[countDoneColours] = crop(croppedImage, b);
                               // doneColours[countDoneColours] = colours;
                                countDoneColours++;
                                if (countDoneColours > 3) { //Environment.Exit(0); 
                                }
                            }
                    }
                }
            //displaying the segments
            /*   pictureBox4.Image = segments[0];
               pictureBox5.Image = segments[1];
               pictureBox6.Image = segments[2];
            if(countSeg==3){
                pictureBox7.Image = segments[3];
            } 
            */
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
