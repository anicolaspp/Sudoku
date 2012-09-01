using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Nikos.SudokuGame
{
    public partial class SudokuGame : Form
    {
        public SudokuGame()
        {
            InitializeComponent();
            sCore = new SudokuCore(panel1);
            InitializeRadioButtons();
        }

        SudokuCore sCore;
        List<RadioButton> rButtons;
        int x = -100, y = -100;
        bool comenzado = false;

        private void InitializeRadioButtons()
        {
            rButtons = new List<RadioButton>(new RadioButton[] 
                { 
                    radioButton0, 
                    radioButton1, 
                    radioButton2, 
                    radioButton3,
                    radioButton4, 
                    radioButton5, 
                    radioButton6, 
                    radioButton7, 
                    radioButton8, 
                    radioButton9 
                });

            rButtons[0].Checked = true;
        }
     
        private void ActivateRButton()
        {
            foreach (RadioButton r in rButtons)
            {
                r.Enabled = true;
            }
        }
        
        private void DeactivateRButton(int[] button)
        {
            for (int i = 0; i < button.Length; i++)
            {
                rButtons[button[i]].Enabled = false;
            }
        }
        
        private void UpGrade()
        {
            ActivateRButton();
            DeactivateRButton(sCore.NoPuedoPoner(sCore.Tx(x), sCore.Ty(y)));
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            x = (e.X / sCore.SeparacionX) * sCore.SeparacionX;
            y = (e.Y / sCore.SeparacionY) * sCore.SeparacionY;

            int v = sCore[sCore.Tx(x), sCore.Ty(y)];
            rButtons[v].Checked = true;

            UpGrade();

            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            sCore.Pinta();
            e.Graphics.DrawEllipse(new Pen(Color.Red, 2), x, y, panel1.Width / 9, panel1.Height / 9);
        }

        private void radioButton0_CheckedChanged(object sender, EventArgs e)
        {
            if (comenzado)
            {
                if (x != -100)
                {
                    int xx = sCore.Tx(x);
                    int yy = sCore.Ty(y);
                    if (sCore.PuedoPoner(xx, yy, 0))
                        sCore.Poner(xx, yy, 0);
                    UpGrade();
                }
                else MessageBox.Show("Debe seleccionar una casilla");
            }
            else comenzado = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 1))
                    sCore.Poner(xx, yy, 1);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 2))
                    sCore.Poner(xx, yy, 2);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 3))
                    sCore.Poner(xx, yy, 3);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 4))
                    sCore.Poner(xx, yy, 4);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 5))
                    sCore.Poner(xx, yy, 5);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 6))
                    sCore.Poner(xx, yy, 6);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 7))
                    sCore.Poner(xx, yy, 7);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 8))
                    sCore.Poner(xx, yy, 8);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (x != -100)
            {
                int xx = sCore.Tx(x);
                int yy = sCore.Ty(y);
                if (sCore.PuedoPoner(xx, yy, 9))
                    sCore.Poner(xx, yy, 9);
                UpGrade();
            }
            else MessageBox.Show("Debe seleccionar una casilla");
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sCore = new SudokuCore(sCore.Contenedor, SudokuCore.Load(openFileDialog1.FileName));
                if (x != -100)
                {
                    UpGrade();
                }
                panel1.Invalidate();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sCore.SaveToFile(saveFileDialog1.FileName, new global::SudokuGame.Utils.SudokuGameState(sCore.OriginalProblem, sCore.Player));
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            sCore.Reset();
            panel1.Invalidate();
            if (x != -100)
            {
                UpGrade();
            }
        }

        private void beginButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea guardar los cambios antes de comenzar un nuevo juego en blanco", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                saveButton.PerformClick();
            }
            sCore = new SudokuCore(sCore.Contenedor);
            panel1.Invalidate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }
}