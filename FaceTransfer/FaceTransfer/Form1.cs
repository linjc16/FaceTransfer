using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DlibDotNet;

namespace FaceTransfer
{

    public partial class Form1 : Form
    {
        private static Bitmap ImgSrc; //源图片
        private static Bitmap ImgGuide; //导引图片
        private static Bitmap ImgTrans; //变形后的图片
        private Bitmap ImgGuideShow;
        private Bitmap ImgSrcShow;
        private double[] PointFace = new double[136];
        private double[] PointFaceGuide = new double[136];
        private int TransMode = 0;
        private int InterpolationMode = 0;
        private int DataLoadMode = 0;
        private int DataLoadModeGuide = 0;
        private string SrcPath;
        private string GuidePath;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        


        //三次B样条插值基函数
        private double CubicBaseG(int i,double t)
        {
            switch (i)
            {
                case 0:
                    return (1 + t * (-3 + t * (3 + t * (-1)))) / 6;
                case 1:
                    return (4 + t * t * (-6 + t * 3)) / 6;
                case 2:
                    return (1 + t * (3 + t * (3 + t * (-3)))) / 6;
                case 3:
                    return t * t * t / 6;
                default:
                    throw new IndexOutOfRangeException();
            }
        }


        //B样条
        //N为网格大小
        private Bitmap BSpline(int N)
        {
            //建立网格
            int[,,] Srcgrid = new int[((ImgSrc.Width + 1) / N) + 5, ((ImgSrc.Height + 1) / N) + 5, 2];

            double[,] XWeightGrid = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
            double[,] YWeightGrid = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];

            double[,] XCount = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
            double[,] YCount = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];

            double[,] temp = new double[((ImgSrc.Width + 1) / N) + 5, ((ImgSrc.Height + 1) / N) + 5];

            int StartCtrl = 0;
            int CountCtrl = 68;
            double XLearningRate = 0.001;
            double YLearningRate = 0.001;
            for (int i = 0; i < Srcgrid.GetLength(0); i++)
                for (int j = 0; j < Srcgrid.GetLength(1); j++) 
                {
                    temp[i, j] = N * i;
                    Srcgrid[i, j, 0] = N * i;
                    Srcgrid[i, j, 1] = N * j;

                }

            //权重初始化为1
            for (int i = 0; i < XWeightGrid.GetLength(0); i++)
                for (int j = 0; j < XWeightGrid.GetLength(1); j++) 
                {
                    XWeightGrid[i, j] = 0.05;
                    YWeightGrid[i, j] = 0.05;
                }



            // 控制点与格点
            double[,] XshiftCtrl = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
            double[,] YshiftCtrl = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
            double[,] XshiftGrid = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
            double[,] YshiftGrid = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];

            //计算每个控制点的位移
            //PointFaceGuide已经最小二乘法移动过
            for (int i = StartCtrl; i < CountCtrl; i++)
            {
                XshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]] = PointFace[i * 2] - PointFaceGuide[i * 2];
                YshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]] = PointFace[i * 2 + 1] - PointFaceGuide[i * 2 + 1];
            }
            
            //更新网格点位移
            for (int i = 0; i < ImgSrc.Width; i++)
                for (int j = 0; j < ImgSrc.Height; j++)
                {

                    int xi = i / N;
                    int yi = j / N;
                    for (int k = 0; k < 4; k++) 
                        for (int l = 0; l < 4; l++)
                        {
                            if (xi - 1 + k < 0 || yi - 1 + l < 0 || xi - 1 + k > Srcgrid.GetLength(0) || yi - 1 + l > Srcgrid.GetLength(1))
                                continue;
                            int XPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 0];
                            int YPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 1];
                            XshiftGrid[XPoint, YPoint] += XshiftCtrl[i, j];
                            YshiftGrid[XPoint, YPoint] += YshiftCtrl[i, j];
                        }
                }

            //梯度下降
            double RecordXLoss = 100;
            double RecordYLoss = 100;
            for (int loop = 0; loop < 20; loop++)
            {
                LOOPCNT.Text = Convert.ToString(loop);
                LOOPCNT.Update();
                double[,] XPartialWeight = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];
                double[,] YPartialWeight = new double[ImgSrc.Width + 5 * N, ImgSrc.Height + 5 * N];

                //求偏导
                for (int i = StartCtrl; i < CountCtrl; i++)
                {
                    int xi = (int)PointFaceGuide[i * 2] / N;
                    int yi = (int)PointFaceGuide[i * 2 + 1] / N;
                    double xu = 1.0 * PointFaceGuide[i * 2] / N - xi;
                    double yu = 1.0 * PointFaceGuide[i * 2 + 1] / N - yi;
                    //求\deta P - \sum...
                    double XSumCubic = 0;
                    double YSumCubic = 0;
                    for (int k = 0; k < 4; k++)
                        for (int l = 0; l < 4; l++) 
                        {
                            if (xi - 1 + k < 0 || yi - 1 + l < 0 || xi - 1 + k > Srcgrid.GetLength(0) || yi - 1 + l > Srcgrid.GetLength(1))
                                continue;
                            int XPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 0];
                            int YPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 1];
                            XSumCubic += CubicBaseG(k, xu) * CubicBaseG(l, yu) * XWeightGrid[XPoint, YPoint] * XshiftGrid[XPoint, YPoint];
                            YSumCubic += CubicBaseG(k, xu) * CubicBaseG(l, yu) * YWeightGrid[XPoint, YPoint] * YshiftGrid[XPoint, YPoint];
                        }

                    for (int k = 0; k < 4; k++)
                        for (int l = 0; l < 4; l++)
                        {
                            if (xi - 1 + k < 0 || yi - 1 + l < 0 || xi - 1 + k > Srcgrid.GetLength(0) || yi - 1 + l > Srcgrid.GetLength(1))
                                continue;
                            int XPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 0];
                            int YPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 1];
                            XPartialWeight[XPoint, YPoint] -= CubicBaseG(k, xu) * CubicBaseG(l, yu)  *
                                XshiftGrid[XPoint, YPoint] * (XshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]] - XSumCubic);

                            YPartialWeight[XPoint, YPoint] -= CubicBaseG(k, xu) * CubicBaseG(l, yu)  *
                                YshiftGrid[XPoint, YPoint] * (YshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]] - YSumCubic);
                        }
                }


                //更新
                for (int i = 0; i < XWeightGrid.GetLength(0); i++)
                    for (int j = 0; j < XWeightGrid.GetLength(1); j++)
                    {
                        XWeightGrid[i, j] = XWeightGrid[i, j] - XLearningRate * (XPartialWeight[i, j] / (CountCtrl - StartCtrl));
                        YWeightGrid[i, j] = YWeightGrid[i, j] - YLearningRate * (YPartialWeight[i, j] / (CountCtrl - StartCtrl));
                    }



                double XLoss = 0;
                double YLoss = 0;
                //计算代价
                for(int i = StartCtrl; i < CountCtrl; i++)
                {
                    int xi = (int)PointFaceGuide[i * 2] / N;
                    int yi = (int)PointFaceGuide[i * 2 + 1] / N;
                    double xu = 1.0 * PointFaceGuide[i * 2] / N - xi;
                    double yu = 1.0 * PointFaceGuide[i * 2 + 1] / N - yi;
                    double XLossTmp = XshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]];
                    double YLossTmp = YshiftCtrl[(int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1]];
                    for (int k = 0; k < 4; k++)
                        for (int l = 0; l < 4; l++)
                        {
                            if (xi - 1 + k < 0 || yi - 1 + l < 0 || xi - 1 + k > Srcgrid.GetLength(0) || yi - 1 + l > Srcgrid.GetLength(1))
                                continue;
                            int XPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 0];
                            int YPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 1];
                            XLossTmp -= CubicBaseG(k, xu) * CubicBaseG(l, yu) * XWeightGrid[XPoint, YPoint] * XshiftGrid[XPoint, YPoint];
                            YLossTmp -= CubicBaseG(k, xu) * CubicBaseG(l, yu) * YWeightGrid[XPoint, YPoint] * YshiftGrid[XPoint, YPoint];
                        }
                    XLoss += XLossTmp * XLossTmp;
                    YLoss += YLossTmp * YLossTmp;
                }

                
                XLOSS.Text = Convert.ToString(XLoss / (2 * (CountCtrl - StartCtrl)));
                XLOSS.Update();
                YLOSS.Text = Convert.ToString(YLoss / (2 * (CountCtrl - StartCtrl)));
                YLOSS.Update();

                if (XLoss / (2 * (CountCtrl - StartCtrl)) - RecordXLoss > 20)
                    XLearningRate /= 2;
                if (YLoss / (2 * (CountCtrl - StartCtrl)) - RecordYLoss > 20)
                    YLearningRate /= 2;
                RecordXLoss = XLoss / (2 * (CountCtrl - StartCtrl));
                RecordYLoss = YLoss / (2 * (CountCtrl - StartCtrl));

              //  if (Math.Abs(XLoss) / (2 * (CountCtrl - StartCtrl)) < 20)
              //      XLearningRate = 0.001;
              //  if (Math.Abs(YLoss) / (2 * (CountCtrl - StartCtrl)) < 45)
              //      YLearningRate = 0.0008;
                if (Math.Abs(XLoss / (2 * (CountCtrl - StartCtrl))) < 1 && Math.Abs(YLoss / (2 * (CountCtrl - StartCtrl))) < 40)
                    break;
                
            }


            //格点与任意点
            Bitmap ImgDst = new Bitmap(ImgSrc.Width, ImgSrc.Height);
            for (int i = 0; i < ImgSrc.Width; i++)
                for (int j = 0; j < ImgSrc.Height; j++)
                {
                    int xi = i / N;
                    int yi = j / N;
                    double xu = 1.0 * i / N - xi;
                    double yu = 1.0 * j / N - yi;
                    double XShiftTmp = 0;
                    double YShiftTmp = 0;
                    for (int k = 0; k < 4; k++) 
                        for (int l = 0; l < 4; l++) 
                        {
                            if (xi - 1 + k < 0 || yi - 1 + l < 0 || xi - 1 + k > Srcgrid.GetLength(0) || yi - 1 + l > Srcgrid.GetLength(1))
                                continue;
                            int XPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 0];
                            int YPoint = Srcgrid[xi - 1 + k, yi - 1 + l, 1];

                            XShiftTmp += CubicBaseG(k, xu) * CubicBaseG(l, yu) * XshiftGrid[XPoint, YPoint] * XWeightGrid[XPoint, YPoint];
                            YShiftTmp += CubicBaseG(k, xu) * CubicBaseG(l, yu) * YshiftGrid[XPoint, YPoint] * YWeightGrid[XPoint, YPoint];

                        }
                    switch(InterpolationMode)
                    {
                        case 0:
                            ImgDst.SetPixel(i, j, Nearest(i + XShiftTmp, j + YShiftTmp, ImgSrc));
                            break;
                        case 1:
                            ImgDst.SetPixel(i, j, Bilinear(i + XShiftTmp, j + YShiftTmp, ImgSrc));
                            break;
                        case 2:
                            ImgDst.SetPixel(i, j, Bicubic(i + XShiftTmp, j + YShiftTmp, ImgSrc));
                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                    
                }

            return ImgDst;

        }


        //最小二乘法（仿射变换）
        private double[] Affine()
        {
            double[,] MatA = new double[68, 3];
            double[,] MatAT = new double[3, 68];
            double[,] MatB = new double[68, 2];
            double[,] ans = new double[3, 2];
            double[,] MatATA = new double[3, 3];
            double[,] MatATB = new double[3, 2];
            double[] PointFaceGuideRot = new double[136];
            for (int i = 0; i < 68; i++)
            {
                //初始化MatA
                MatA[i, 0] = PointFaceGuide[i * 2];
                MatA[i, 1] = PointFaceGuide[i * 2 + 1];
                MatA[i, 2] = 1;
                //初始化MatA转置
                MatAT[0, i] = PointFaceGuide[i * 2];
                MatAT[1, i] = PointFaceGuide[i * 2 + 1];
                MatAT[2, i] = 1;
                //初始化MatB
                MatB[i, 0] = PointFace[i * 2];
                MatB[i, 1] = PointFace[i * 2 + 1];
            }
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    for(int k = 0; k < 68; k++)
                        MatATA[i, j] += MatAT[i, k] * MatA[k, j];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 68; k++)
                        MatATB[i, j] += MatAT[i, k] * MatB[k, j];

            double[,] MatATAInv = new double[3, 3];
            MatATAInv = MatInverse(MatATA);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 3; k++)
                        ans[i, j] += MatATAInv[i, k] * MatATB[k, j];
            for (int i = 0; i < 68; i++) 
            {
                PointFaceGuideRot[i * 2] = ans[0, 0] * PointFaceGuide[i * 2] + ans[1, 0] * PointFaceGuide[i * 2 + 1] + ans[2, 0];
                PointFaceGuideRot[i * 2 + 1] = ans[0, 1] * PointFaceGuide[i * 2] + ans[1, 1] * PointFaceGuide[i * 2 + 1] + ans[2, 1]; 
            }
            return PointFaceGuideRot;
            
        }
        //最小二乘法（无旋转）
        private double[] LeastSquare()
        {
            double Ax = 0, Bx = 0, Cx = 0, Dx = 0;
            double Ay = 0, By = 0, Cy = 0, Dy = 0;
            for (int i = 0; i < 68; i++)
            {
                Ax += PointFaceGuide[i * 2] * PointFaceGuide[i * 2];
                Bx += PointFaceGuide[i * 2];
                Cx += PointFace[i * 2] * PointFaceGuide[i * 2];
                Dx += PointFace[i * 2];

                Ay += PointFaceGuide[i * 2 + 1] * PointFaceGuide[i * 2 + 1];
                By += PointFaceGuide[i * 2 + 1];
                Cy += PointFace[i * 2 + 1] * PointFaceGuide[i * 2 + 1];
                Dy += PointFace[i * 2 + 1];
            }
            double[] ans = new double[4];
            if (Ax * 68 - Bx * Bx != 0)
            {
                ans[0] = (Cx - Bx * Dx / 68) / (Ax - Bx * Bx / 68);
                ans[1] = Dx / 68 - ans[0] * Bx / 68;
            }
            else
            {
                ans[0] = 1;
                ans[1] = 0;
            }
            if(Ay * 68 - By * By != 0)
            {
                ans[2] = (Cy - By * Dy / 68) / (Ay  - By * By / 68);
                ans[3] = Dy / 68 - ans[2] * By / 68;
            }
            else
            {
                ans[2] = 1;
                ans[3] = 0;
            }
            return ans;

        }

        //U函数
        private double FunctionU(double r2)
        {
            if (r2 != 0)
                return r2 * Math.Log(r2);
            else
                return 0;
        }

        //TPS 变形
        private Bitmap TPS()
        {
            double[,] MatL = new double[71, 71];
            double[,] MatY = new double[71, 2];
            double[,] MatPara = new double[71, 2];
            double[,] MatLInv = new double[71, 71];
            double[] MatProj = new double[136];
            // 填充矩阵K
            for (int i = 0; i < 68; i++) 
            {
                for(int j = 0; j < 68; j++)
                {
                    if (i == j)
                        MatL[i, j] = 0;
                    else
                    {
                        double r2 = (PointFaceGuide[2 * i] - PointFaceGuide[2 * j])* (PointFaceGuide[2 * i] - PointFaceGuide[2 * j]) + 
                            (PointFaceGuide[2 * i + 1] - PointFaceGuide[2 * j + 1])* (PointFaceGuide[2 * i + 1] - PointFaceGuide[2 * j + 1]);
                        MatL[i, j] = FunctionU(r2);
                    }
                }
            }
            // 填充矩阵P
            for (int i = 0; i < 68; i++)
                for (int j = 68; j < 71; j++)
                {
                    if (j == 68)
                        MatL[i, j] = 1;
                    else if (j == 69)
                        MatL[i, j] = PointFaceGuide[2 * i];
                    else if (j == 70)
                        MatL[i, j] = PointFaceGuide[2 * i + 1];
                }
            // 填充矩阵P转置
            for (int i = 68; i < 71; i++)
                for(int j = 0; j< 68; j++)
                {
                    if (i == 68)
                        MatL[i, j] = 1;
                    else if (i == 69)
                        MatL[i, j] = PointFaceGuide[2 * j];
                    else if (i == 70)
                        MatL[i, j] = PointFaceGuide[2 * j + 1];
                }



            // 填充矩阵Y
            for (int i = 0; i < 68; i++)
                for (int j = 0; j < 2; j++) 
                {
                    if (j == 0)
                        MatY[i, j] = PointFace[2 * i];
                    if (j == 1)
                        MatY[i, j] = PointFace[2 * i + 1]; 
                }


            //求解方程
            MatPara = MatSolve(MatL, MatY);
           // MatLInv = MatInverse(MatL); //逆矩阵
           // path = "C:/Users/lenovo/Desktop/picture/MatLInv.csv";
           // Print(path, MatLInv);
           // for (int i = 0; i < 71; i++)
           // {
           //     for (int j = 0; j < 71; j++)
           //     {
           //         MatPara[i, 0] += MatLInv[i, j] * MatY[j, 0];
           //         MatPara[i, 1] += MatLInv[i, j] * MatY[j, 1];
           //     }
           // }

            //得到参数后代入
            Bitmap ImgDst = new Bitmap(ImgSrc.Width, ImgSrc.Height);
            for (int i = 0; i < ImgSrc.Width; i++)
                for (int k = 0; k < ImgSrc.Height; k++) 
                {
                    double[] array = new double[71];
                    array[0] = 1;
                    array[1] = i;       //x
                    array[2] = k;       //y
                    for (int j = 3; j < 71; j++)
                    {
                        array[j] = FunctionU(Math.Pow(PointFaceGuide[(j - 3) * 2] - array[1], 2) + Math.Pow(PointFaceGuide[(j - 3) * 2 + 1] - array[2], 2));
                    }
                    double SumX = 0;
                    double SumY = 0;
                    SumX += array[0] * MatPara[68, 0];
                    SumX += array[1] * MatPara[69, 0];
                    SumX += array[2] * MatPara[70, 0];
                    SumY += array[0] * MatPara[68, 1];
                    SumY += array[1] * MatPara[69, 1];
                    SumY += array[2] * MatPara[70, 1];
                    for (int j = 3; j < 71; j++) 
                    {
                        SumX += array[j] * MatPara[j - 3, 0];
                        SumY += array[j] * MatPara[j - 3, 1];
                    }
                    switch(InterpolationMode)
                    {
                        case 0:
                            ImgDst.SetPixel(i, k, Nearest(SumX, SumY, ImgSrc));
                            break;
                        case 1:
                            ImgDst.SetPixel(i, k, Bilinear(SumX, SumY, ImgSrc));
                            break;
                        case 2:
                            ImgDst.SetPixel(i, k, Bicubic(SumX, SumY, ImgSrc));
                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                    
                    
                }  

            return ImgDst;


        }

        //矩阵某两行交换
        private double[,] MatSwap(double[,] Mat, int Row1, int Row2)
        {
            int size = Mat.GetLength(0);
            for(int i = 0; i < size; i++)
            {
                double temp = Mat[Row1, i];
                Mat[Row1, i] = Mat[Row2, i];
                Mat[Row2, i] = temp;
            }
            return Mat;
        }

        //矩阵求逆
        private double[,] MatInverse(double [,] Mat)
        {
            int size = Mat.GetLength(0);
            double[,] ArgMat = new double[2 * size + 1, 2 * size + 1];
            //左上角矩阵A
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++) 
                    ArgMat[i, j] = Mat[i, j];
            for (int i = 0; i < size; i++)
                for (int j = size; j < 2 * size + 1; j++)
                {
                    if ((j - i) == size) 
                    {
                        ArgMat[i, j] = 1.0;
                    }
                }
            //得到逆矩阵
            for(int i = 0; i < size; i++)
            {
                if(ArgMat[i,i] != 1)
                {
                    if (ArgMat[i, i] == 0)
                    {
                        for (int j = i; j < size; j++)
                        {
                            if (ArgMat[j, i] != 0)
                            {
                                ArgMat = MatSwap(ArgMat, j, i);
                                break;
                            }
                        }
                    }

                    double bs = ArgMat[i, i];
                    ArgMat[i, i] = 1;
                    for (int p = i + 1; p < 2 * size; p++)
                        ArgMat[i, p] /= bs;
                    
                }
                for(int q = 0; q < size; q++)
                {
                    if (q != i)
                    {
                        double bs = ArgMat[q, i];
                        for (int p = 0; p < 2 * size; p++)
                        {
                            ArgMat[q, p] -= bs * ArgMat[i, p];
                        }
                    }
                    else
                        continue;
                }
            }
            double[,] MatInv = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = size; j < 2 * size; j++) 
                {
                    MatInv[i, j - size] = ArgMat[i, j];
                }
            return MatInv;
        }


        //增广矩阵求解
        private double[,] MatSolve(double[,] Mat,double[,] Mat2)
        {
            int size = Mat.GetLength(0);
            double[,] ArgMat = new double[2 * size + 1, 2 * size + 1];
            //左上角矩阵A
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    ArgMat[i, j] = Mat[i, j];

            for (int i = 0; i < size; i++)
                for (int j = size; j < size + 2; j++)
                    ArgMat[i, j] = Mat2[i, j - size];

            //得到逆矩阵
            for (int i = 0; i < size; i++)
            {
                if (ArgMat[i, i] != 1)
                {
                    if (ArgMat[i, i] == 0)
                    {
                        for (int j = i; j < size; j++)
                        {
                            if (ArgMat[j, i] != 0)
                            {
                                ArgMat = MatSwap(ArgMat, j, i);
                                break;
                            }
                        }
                    }

                    double bs = ArgMat[i, i];
                    ArgMat[i, i] = 1;
                    for (int p = i + 1; p < 2 * size; p++)
                        ArgMat[i, p] /= bs;

                }
                for (int q = 0; q < size; q++)
                {
                    if (q != i)
                    {
                        double bs = ArgMat[q, i];
                        for (int p = 0; p < 2 * size; p++)
                        {
                            ArgMat[q, p] -= bs * ArgMat[i, p];
                        }
                    }
                    else
                        continue;
                }
            }
            double[,] MatSolve = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = size; j < 2 * size; j++)
                {
                    MatSolve[i, j - size] = ArgMat[i, j];
                }
            return MatSolve;
        }

        //最近邻插值
        //x,y为变换后的图像所对应的原图像的坐标
        private Color Nearest(double x, double y, Bitmap Img)
        {

            if (x < 0 || y < 0 || (int)(x+0.5) >= Img.Width || (int)(y+0.5) >= Img.Height) 
            {
                return Color.FromArgb(0,0,0);
            }
            else
            {
                int ox = (int)(x + 0.5);
                int oy = (int)(y + 0.5);
                if (ox < 0) ox = 0;
                if (oy < 0) oy = 0;
                return Img.GetPixel(ox, oy);
            }

        }
        //双线性插值
        private Color Bilinear(double x, double y, Bitmap Img)
        {
            if (x < 0 || y < 0 || x + 1 >= Img.Width || y + 1 >= Img.Height)
            {
                return Color.FromArgb(0, 0, 0);
            }
            else
            {
                // xi,yi为整数部分，xu,yu为小数部分
                double xi = (int)Math.Floor(x);
                double xu = x - xi;
                double yi = (int)Math.Floor(y);
                double yu = y - yi;
                //获取临近的四个像素点
                Color Pixel_1 = Img.GetPixel((int)xi, (int)yi);                  // f(xi,yi)
                Color Pixel_2 = Img.GetPixel((int)(xi + 1), (int)yi);            // f(xi+1,yi)
                Color Pixel_3 = Img.GetPixel((int)xi, (int)(yi + 1));            // f(xi,yi+1)
                Color Pixel_4 = Img.GetPixel((int)(xi + 1), (int)(yi + 1));      // f(xi+1,yi+1)
                //对三个通道每个通道进行双线性插值
                double oRed = (1 - xu) * (1 - yu) * Pixel_1.R + xu * (1 - yu) * Pixel_2.R + (1 - xu) * yu * Pixel_3.R + xu * yu * Pixel_4.R;
                double oGreen = (1 - xu) * (1 - yu) * Pixel_1.G + xu * (1 - yu) * Pixel_2.G + (1 - xu) * yu * Pixel_3.G + xu * yu * Pixel_4.G;
                double oBlue = (1 - xu) * (1 - yu) * Pixel_1.B + xu * (1 - yu) * Pixel_2.B + (1 - xu) * yu * Pixel_3.B + xu * yu * Pixel_4.B;

                return Color.FromArgb((int)oRed, (int)oGreen, (int)oBlue);
            }
        }
        private double BicubicFuncS(double u)
        {
            if (Math.Abs(u) <= 1)
                return 1 - 2 * Math.Pow(Math.Abs(u), 2) + Math.Pow(Math.Abs(u), 3);
            else if (Math.Abs(u) > 1 && Math.Abs(u) < 2)
                return 4 - 8 * Math.Abs(u) + 5 * Math.Pow(Math.Abs(u), 2) - Math.Pow(Math.Abs(u), 3);
            else
                return 0;
        }

        //双三次插值 
        private Color Bicubic(double x, double y,Bitmap Img)
        {
            if (x < 0 || y < 0 || x >= Img.Width || y >= Img.Height)
            {
                return Color.FromArgb(0, 0, 0);
            }
            else
            {
                // xi,yi为整数部分，xu,yu为小数部分
                double xi = (int)Math.Floor(x);
                double xu = x - xi;
                double yi = (int)Math.Floor(y);
                double yu = y - yi;
                //定义原图像像素矩阵
                double[,] PixelValueR = new double[4, 4];
                double[,] PixelValueG = new double[4, 4];
                double[,] PixelValueB = new double[4, 4];
                //定义A矩阵与C矩阵
                double[] MatA = new double[4];
                double[] MatC = new double[4];

                // 读取(xi,yi)周围16个点的RGB值
                for (int i = 0; i < 4; ++i)
                    for (int j = 0; j < 4; ++j)
                    {
                        if (xi - 1 + i < 0 || yi - 1 + j < 0 || xi - 1 + i >= Img.Width || yi - 1 + j >= Img.Height)
                        {
                            PixelValueR[i, j] = 0;
                            PixelValueG[i, j] = 0;
                            PixelValueB[i, j] = 0;
                        }
                        else
                        {
                            PixelValueR[i, j] = Img.GetPixel((int)xi - 1 + i, (int)yi - 1 + j).R;
                            PixelValueG[i, j] = Img.GetPixel((int)xi - 1 + i, (int)yi - 1 + j).G;
                            PixelValueB[i, j] = Img.GetPixel((int)xi - 1 + i, (int)yi - 1 + j).B;
                        }
                    }
                // 赋予A矩阵和C矩阵值
                for (int i = 0; i < 4; i++)
                {
                    MatA[i] = BicubicFuncS(xu + 1 - i);
                    MatC[i] = BicubicFuncS(yu + 1 - i);
                }

                //进行矩阵计算，得出双三次插值后的结果
                double[,] MidAns = new double[3, 4];
                for (int i = 0; i < 4; ++i)
                {
                    MidAns[0, i] = 0;
                    MidAns[1, i] = 0;
                    MidAns[2, i] = 0;
                    for (int j = 0; j < 4; ++j)
                    {
                        MidAns[0, i] += MatA[j] * PixelValueR[j, i];
                        MidAns[1, i] += MatA[j] * PixelValueG[j, i];
                        MidAns[2, i] += MatA[j] * PixelValueB[j, i];
                    }
                }
                double oRed = 0;
                double oGreen = 0;
                double oBlue = 0;
                for (int i = 0; i < 4; i++)
                {
                    oRed += MidAns[0, i] * MatC[i];
                    oGreen += MidAns[1, i] * MatC[i];
                    oBlue += MidAns[2, i] * MatC[i];
                }
                if (oRed > 255)     oRed = 255;
                if (oGreen > 255)   oGreen = 255;
                if (oBlue > 255)    oBlue = 255;
                if (oRed < 0)       oRed = 0;
                if (oGreen < 0)     oGreen = 0;
                if (oBlue < 0)      oBlue = 0;
                return Color.FromArgb((int)oRed, (int)oGreen, (int)oBlue);


            }
        }

      
     
        //载入图片
        private void LoadButton_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = @"Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.jpg)|*.jpg|所有合适文件(*.bmp,*.jpg)|*.bmp;*.jpg";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                SrcPath = openFileDialog1.FileName;
                ImgSrc = (Bitmap)Image.FromFile(path, false);
                ImgSrcShow = (Bitmap)Image.FromFile(path, false); 
                pictureBox1.Image = ImgSrc.Clone() as Image;
                this.Invalidate();
                DataButton.Enabled = true;
            }
        }

        private void LoadButton2_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = @"Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.jpg)|*.jpg|所有合适文件(*.bmp,*.jpg)|*.bmp;*.jpg";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                GuidePath = openFileDialog1.FileName;
                ImgGuide = (Bitmap)Image.FromFile(path, false);
                ImgGuideShow = (Bitmap)Image.FromFile(path, false);
                pictureBox2.Image = ImgGuide.Clone() as Image;
                this.Invalidate();
                DataButton2.Enabled = true;
            }
        }

        //保存图片
        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool isSave = true;
            saveFileDialog1.Filter = @"Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.jpg)|*.jpg|所有合适文件(*.bmp,*.jpg)|*.bmp;*.jpg";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName.ToString();
                if(fileName != null && fileName != "")
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();
                    System.Drawing.Imaging.ImageFormat imgformat = null;
                    if (fileExtName != "")
                    {
                        switch(fileExtName)
                        {
                            case "jpg":
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            default:
                                MessageBox.Show("只能存为jpg或bmp格式");
                                isSave = false;
                                break;
                        }
                    }
                    //默认存为jpg格式
                    if(imgformat == null)
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    if(isSave)
                    {
                        try
                        {
                            this.pictureBox3.Image.Save(fileName, imgformat);
                            MessageBox.Show("图片已经成功保存!");
                        }
                        catch
                        {
                            MessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                        }
                        
                    }
                }

            }
        }



        private void RunButton_Click(object sender, EventArgs e)
        {

            double[] PointTempGuide = new double[136];
            PointTempGuide = PointFaceGuide;
            
            PointFaceGuide = Affine();          //仿射变换
            switch (TransMode)
            {
                case 0:
                    ImgTrans = TPS();
                    break;
                case 1:
                    int BSplineN = new int();
                    if (Math.Abs(PointFace[0] - PointFace[16 * 2]) + Math.Abs(PointFace[1] - PointFace[16 * 2 + 1]) < 200)
                        BSplineN = 13;
                    else
                        BSplineN = 20;
                    ImgTrans = BSpline(BSplineN);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
            PointFaceGuide = PointTempGuide;
            pictureBox3.Image = ImgTrans.Clone() as Image;
            this.Invalidate();


        }
        //读取人脸关键点数据
        private void DataButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = @"文本文件(*.txt)|*.txt";
            openFileDialog1.RestoreDirectory = true;
            bool isLoad = false;

            switch (DataLoadMode)
            {
                case 0:
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string path = openFileDialog1.FileName;
                        StreamReader sr = new StreamReader(path, Encoding.Default);
                        String[] data = { };
                        char[] separator = { '\n', '\r', ' ' };
                        data = sr.ReadToEnd().Split(separator);
                        for (int i = 0; i < 68 * 2; i++)
                            PointFace[i] = (int)Convert.ToSingle(data[i]);
                        isLoad = true;
                    }
                    break;
                case 1:
                    var detector = Dlib.GetFrontalFaceDetector();
                    var img = Dlib.LoadImage<RgbPixel>(SrcPath);
                    Dlib.PyramidUp(img);
                    var dets = detector.Operator(img);
                    var temp = dets.Length;
                    var shapes = new List<FullObjectDetection>();
                    var sp = ShapePredictor.Deserialize("./model.dat");
                    foreach (var rect in dets)
                    {
                        var shape = sp.Detect(img, rect);
                        for (int i = 0; i < 68; i++)
                        {
                            PointFace[i * 2] = shape.GetPart((uint)i).X / 2;
                            PointFace[i * 2 + 1] = shape.GetPart((uint)i).Y / 2;
                        }
                        if (shape.Parts > 2)
                        {
                            shapes.Add(shape);
                        }
                    }
                    isLoad = true;
                        break;
                default:
                    throw new IndexOutOfRangeException();
            }
            if(isLoad)
            {
                //显示标记点
                var g = Graphics.FromImage(ImgSrcShow);
                for (int i = 0; i < 68; i++)
                    g.FillEllipse(new SolidBrush(Color.FromArgb(255, 255, 220)), (int)PointFace[i * 2], (int)PointFace[i * 2 + 1], 4, 4);
                g.Dispose();
                pictureBox1.Image = ImgSrcShow.Clone() as Image;
                this.Invalidate();
                if (DataButton.Enabled == true && DataButton2.Enabled == true)
                    RunButton.Enabled = true;
            }



        }

        private void DataButton2_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = @"文本文件(*.txt)|*.txt";
            openFileDialog1.RestoreDirectory = true;
            bool isLoad = false;

            switch(DataLoadModeGuide)
            {
                case 0:
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string path = openFileDialog1.FileName;
                        StreamReader sr = new StreamReader(path, Encoding.Default);
                        String[] data = { };
                        char[] separator = { '\n', '\r', ' ' };
                        data = sr.ReadToEnd().Split(separator);
                        for (int i = 0; i < 68 * 2; i++)
                            PointFaceGuide[i] = (int)Convert.ToSingle(data[i]);
                        isLoad = true;
                    }
                    break;
                case 1:
                    var detector = Dlib.GetFrontalFaceDetector();
                    var img = Dlib.LoadImage<RgbPixel>(GuidePath);
                    Dlib.PyramidUp(img);
                    var dets = detector.Operator(img);
                    //var temp = dets.Length;
                    var shapes = new List<FullObjectDetection>();
                    var sp = ShapePredictor.Deserialize("./model.dat");
                    foreach (var rect in dets)
                    {
                        var shape = sp.Detect(img, rect);
                        for (int i = 0; i < 68; i++)
                        {
                            PointFaceGuide[i * 2] = shape.GetPart((uint)i).X / 2;
                            PointFaceGuide[i * 2 + 1] = shape.GetPart((uint)i).Y / 2;
                        }
                        if (shape.Parts > 2)
                        {
                            shapes.Add(shape);
                        }
                    }
                    isLoad = true;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
            if(isLoad)
            {
                //显示标记点
                var g = Graphics.FromImage(ImgGuideShow);
                for (int i = 0; i < 68; i++)
                    g.FillEllipse(new SolidBrush(Color.FromArgb(255, 255, 220)), (int)PointFaceGuide[i * 2], (int)PointFaceGuide[i * 2 + 1], 4, 4);
                g.Dispose();
                pictureBox2.Image = ImgGuideShow.Clone() as Image;
                if (DataButton.Enabled == true && DataButton2.Enabled == true)
                    RunButton.Enabled = true;
            }

        }

        public static void Print(String path, double[,] mat)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    sw.Write(mat[i, j]);
                    sw.Write(',');
                }
                sw.Write("\r\n");
            }
            sw.Close();
            fs.Close();
        }

        public static void PrintArray(String path, double[] array)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < array.Length; i++)
            {
                sw.Write(array[i]);
                sw.Write(',');
                sw.Write("\r\n");
            }
            sw.Close();
            fs.Close();
        }

        public static void PrintArrayInt(String path, int[] array)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < array.Length; i++)
            {
                sw.Write(array[i]);
                sw.Write(',');
                sw.Write("\r\n");
            }
            sw.Close();
            fs.Close();
        }

        private void radioTPS_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTPS.Checked)
                TransMode = 0;
        }

        private void radioBSpline_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBSpline.Checked)
                TransMode = 1;
        }

        private void radioNearest_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNearest.Checked)
                InterpolationMode = 0;
        }

        private void radioBilinear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBilinear.Checked)
                InterpolationMode = 1;
        }

        private void radioBicubic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBicubic.Checked)
                InterpolationMode = 2;
        }



        private void radioLoadMannul_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLoadMannul.Checked)
                DataLoadMode = 0;
        }

        private void radioLoadAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLoadAuto.Checked)
                DataLoadMode = 1;
        }

        private void radioLoadMannul1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLoadMannul1.Checked)
                DataLoadModeGuide = 0;
        }

        private void radioLoadAuto1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLoadAuto1.Checked)
                DataLoadModeGuide = 1;
        }
    }
}
