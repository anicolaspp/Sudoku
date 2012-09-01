using System;
using System.Collections.Generic;
using System.Text;
using SudokuGame.Utils;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace Nikos.SudokuGame
{
    /// <summary>
    /// Representa el motor o nucleo del Sudoku
    /// </summary>
    public class SudokuCore : ICloneable
    {
        int[,] tablero;
        Control miControl;
        Pen miPluma;
        ArrayList player, original;
        /// <summary>
        /// contiene el estado en que se ha creado el sudoku, si se ha creado vacio entonces este es null
        /// </summary>
        SudokuGameState miState;
        int sAncho;
        int sAlto;

        /// <summary>
        /// Representa las longitudes del Sudoku 
        /// </summary>
        public const int maxSize = 9;
        /// <summary>
        /// Representa las longitudes de los cuadritos que conforman al Sudoku
        /// </summary>
        public const int minSize = 3;

        private void PintaTablero()
        {
            Graphics gr = miControl.CreateGraphics();
            gr.Clear(miControl.BackColor);

            for (int i = 0; i < miControl.Width; i++)
            {
                if (i % sAncho == 0)
                {
                    gr.DrawLine(miPluma, i, 0, i, miControl.Height);
                }
            }
            for (int i = 0; i < miControl.Height; i++)
            {
                if (i % sAlto == 0)
                {
                    gr.DrawLine(miPluma, 0, i, miControl.Width, i);
                }
            }

            //RESALTANDO LOS BORDES
            float anchoExterior = 10;
            float anchoInterior = 4;
            Pen bordeExterior = new Pen(Color.MidnightBlue, anchoExterior);
            Pen bordeInterior = new Pen(Color.MidnightBlue, anchoInterior);

            gr.DrawLine(bordeExterior, 0, 0, miControl.Height, 0);
            gr.DrawLine(bordeExterior, 0, 0, 0, miControl.Width);
            gr.DrawLine(bordeExterior, 0, miControl.Width, miControl.Height, miControl.Width);
            gr.DrawLine(bordeExterior, miControl.Height, miControl.Width, miControl.Height, 0);

            int j = 0;
            while (j <= miControl.Width)
            {
                j += sAncho * 3;
                gr.DrawLine(bordeInterior, j, 0, j, miControl.Height);
                gr.DrawLine(bordeInterior, 0, j, miControl.Width, j);
            }

            gr.Dispose();
        }
        private void PintaNumeros()
        {
            Graphics gr = miControl.CreateGraphics();

            Font f = new Font("Arial", 30);
            SolidBrush s = new SolidBrush(Color.Red);
            foreach (SudokuCell cell in player)
            {
                gr.DrawString(cell.Value.ToString(), f, s, cell.X * sAncho, cell.Y * sAlto);
            }
            s = new SolidBrush(Color.Black);
            foreach (SudokuCell cell in original)
            {
                gr.DrawString(cell.Value.ToString(), f, s, cell.X * sAncho, cell.Y * sAlto);
            }

            gr.Dispose();
        }
        private int CualBloque(int x, int y)
        {
            if (x >= 0 && x <= 2 && y >= 0 && y <= 2) return 1;
            if (x >= 0 && x <= 2 && y >= 3 && y <= 5) return 2;
            if (x >= 0 && x <= 2 && y >= 6 && y <= 8) return 3;

            if (x >= 3 && x <= 5 && y >= 0 && y <= 2) return 4;
            if (x >= 3 && x <= 5 && y >= 3 && y <= 5) return 5;
            if (x >= 3 && x <= 5 && y >= 6 && y <= 8) return 6;

            if (x >= 6 && x <= 8 && y >= 0 && y <= 2) return 7;
            if (x >= 6 && x <= 8 && y >= 3 && y <= 5) return 8;
            if (x >= 6 && x <= 8 && y >= 6 && y <= 8) return 9;

            return 0;
        }
        private int LaTengo(int x, int y)
        {
            for (int i = 0; i < player.Count; i++)
            {
                if (((SudokuCell)player[i]).X == x && ((SudokuCell)player[i]).Y == y)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Crear un nuevo Sudoku
        /// </summary>
        /// <param name="Contenedor">Control deonde se va a pintar el Sudoku</param>
        public SudokuCore(Control Contenedor)
        {
            tablero = new int[9, 9];
            this.miControl = Contenedor;
            this.miPluma = new Pen(Color.Blue, 2);
            this.sAncho = miControl.Width / 9;
            this.sAlto = miControl.Height / 9;
            this.player = new ArrayList();
            this.original = new ArrayList();
        }
        /// <summary>
        /// Crear un nuevo Sudoku a partir de un estado dado
        /// </summary>
        /// <param name="Contenedor">Control deonde se va a pintar el Sudoku</param>
        /// <param name="state">Estado inicial del Sudoku</param>
        public SudokuCore(Control Contenedor, SudokuGameState state)
            : this(Contenedor)
        {
            this.original = new ArrayList(state.OriginalProblem);
            this.player = new ArrayList(state.PlayerCells);
            this.miState = state;

            foreach (SudokuCell cell in state.OriginalProblem)
            {
                tablero[cell.X, cell.Y] = cell.Value;
            }
            foreach (SudokuCell cell in state.PlayerCells)
            {
                tablero[cell.X, cell.Y] = cell.Value;
            }
        }

        #region IOHelper

        /// <summary>
        /// Carga un estado de Sudoku desde un archivo
        /// </summary>
        /// <param name="NombreArchivo">Nombre del archivo</param>
        /// <returns>El Sudoku que se ha cargado</returns>
        public static SudokuGameState Load(string NombreArchivo)
        {
            return SudokuIOHelper.LoadFromFile(NombreArchivo);
        }
        /// <summary>
        /// Guarda un Sudoku hacia un archivo
        /// </summary>
        /// <param name="NombreArchivo">Nombre del archivo</param>
        /// <param name="gameState">Estado de juego a guardar</param>
        public void SaveToFile(string NombreArchivo, SudokuGameState gameState)
        {
            SudokuIOHelper.SaveToFile(NombreArchivo, gameState);
        }

        #endregion

        #region Funciones para comvertir coordenadas de control a coordenadas de tablero

        /// <summary>
        /// Convierte la coordenada x del control en coordenada de tablero
        /// </summary>
        /// <param name="x">coordenada de control</param>
        /// <returns>coordenada de tablero</returns>
        public int Tx(int x)
        {
            return x / SeparacionX;
        }
        /// <summary>
        /// Convierte la coordenada y del control en coordenada de tablero
        /// </summary>
        /// <param name="x">coordenada de control</param>
        /// <returns>coordenada de tablero</returns>
        public int Ty(int y)
        {
            return y / SeparacionY;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Reinicia al Sudoku en el estado en el que se ha cargado o creado
        /// </summary>
        public void Reset()
        {
            if (miState != null)
            {
                this.player = new ArrayList(miState.PlayerCells);

                foreach (SudokuCell cell in miState.OriginalProblem)
                {
                    tablero[cell.X, cell.Y] = cell.Value;
                }
                foreach (SudokuCell cell in miState.PlayerCells)
                {
                    tablero[cell.X, cell.Y] = cell.Value;
                }
            }
            else
            {
                this.player = new ArrayList();
                tablero = new int[9, 9];
            }
        }
        /// <summary>
        /// Pinta el Sudoku en el control que lo contiene
        /// </summary>
        public void Pinta()
        {
            PintaTablero();
            PintaNumeros();
        }
        /// <summary>
        /// Pone un valor en las coordenadas dadas
        /// </summary>
        /// <param name="x">coordenada x</param>
        /// <param name="y">coordenada y</param>
        /// <param name="valor">valor a poner</param>
        public void Poner(int x, int y, int valor)
        {
            tablero[x, y] = valor;
            int pos = LaTengo(x, y);
            if (pos >= 0) player.RemoveAt(pos);
            if (valor != 0) player.Add(new SudokuCell(x, y, valor));

            miControl.Invalidate();
        }
        /// <summary>
        /// Verifica que se pueda pones un valor en una coordenada
        /// </summary>
        /// <param name="x">coordenada x</param>
        /// <param name="y">coordenada y</param>
        /// <param name="valor">valor a poner</param>
        /// <returns>true si se puede poner, false en caso contrario</returns>
        public bool PuedoPoner(int x, int y, int valor)
        {
            if (valor != 0)
                foreach (SudokuCell cell in original)
                    if (cell.X == x && cell.Y == y) return false;

            if (valor == 0) return true;
            for (int i = 0; i < 9; i++)
            {
                if (tablero[x, i] == valor || tablero[i, y] == valor) return false;
            }

            int bloque = CualBloque(x, y);

            if (bloque == 1)
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 2)
                for (int i = 0; i < 3; i++)
                    for (int j = 3; j < 6; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 3)
                for (int i = 0; i < 3; i++)
                    for (int j = 6; j < 9; j++)
                        if (tablero[i, j] == valor) return false;

            if (bloque == 4)
                for (int i = 3; i < 6; i++)
                    for (int j = 0; j < 3; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 5)
                for (int i = 3; i < 6; i++)
                    for (int j = 3; j < 6; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 6)
                for (int i = 3; i < 6; i++)
                    for (int j = 6; j < 9; j++)
                        if (tablero[i, j] == valor) return false;

            if (bloque == 7)
                for (int i = 6; i < 9; i++)
                    for (int j = 0; j < 3; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 8)
                for (int i = 6; i < 9; i++)
                    for (int j = 3; j < 6; j++)
                        if (tablero[i, j] == valor) return false;
            if (bloque == 9)
                for (int i = 6; i < 9; i++)
                    for (int j = 6; j < 9; j++)
                        if (tablero[i, j] == valor) return false;

            return true;
        }
        /// <summary>
        /// Obtiene los valores que no se pueden poner en una coordenada
        /// </summary>
        /// <param name="x">coordenada x</param>
        /// <param name="y">coordenada y</param>
        /// <returns>Valore obtenidos</returns>
        public int[] NoPuedoPoner(int x, int y)
        {
            ArrayList result = new ArrayList();
            for (int i = 1; i < 10; i++)
            {
                if (!PuedoPoner(x, y, i))
                    result.Add(i);
            }

            return (int[])result.ToArray(typeof(int));
        }

        #endregion
        
        #region Propiedades

        /// <summary>
        /// Obtiene la separacion entre las cuadriculas del tablero horizontalmente en coordenadas de control
        /// </summary>
        public int SeparacionX
        {
            get { return sAlto; }
        }
        /// <summary>
        /// Obtiene la separacion entre las cuadriculas del tablero verticalmente en coordenadas de control
        /// </summary>
        public int SeparacionY
        {
            get { return sAncho; }
        }
        /// <summary>
        /// Obtiene un array con celdas del problema original
        /// </summary>
        public SudokuCell[] OriginalProblem
        {
            get { return (SudokuCell[])original.ToArray(typeof(SudokuCell)); }
        }
        /// <summary>
        /// Obtiene un array con celdas jugadas por el usuario
        /// </summary>
        public SudokuCell[] Player
        {
            get { return (SudokuCell[])player.ToArray(typeof(SudokuCell)); }
        }
        /// <summary>
        /// Retorna true si el Sudoku esta completo, false en caso contrario
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return (player.Count + original.Count == 81);
            }
        }
        /// <summary>
        /// Retorma el control donde esta contenido
        /// </summary>
        public Control Contenedor
        {
            get { return miControl; }
        }
        /// <summary>
        /// Retorna el estado actual del Sudoku
        /// </summary>
        public SudokuGameState State
        {
            get
            {
                return new SudokuGameState(OriginalProblem, Player);
            }
        }

        /// <summary>
        /// Obtiene un valor del tablero
        /// </summary>
        /// <param name="x">coordenada x</param>
        /// <param name="y">coordenada y</param>
        /// <returns>Valor del tablero para coordenadas dadas</returns>
        public int this[int x, int y]
        {
            get { return tablero[x, y]; }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Retorna una copia del Sudoku
        /// </summary>
        /// <returns>La copia del Sudoku</returns>
        public object Clone()
        {
            if (player.Count == 0 & original.Count == 0)
            {
                return new SudokuCore(miControl);
            }
            else
            {
                return new SudokuCore(miControl, State);
            }
        }

        #endregion

        public override string ToString()
        {
            return "This class is the sudoku game's core and it contains the functions to manipulate it";
        }
    }
}