using System;
using System.Collections.Generic;
using System.IO;

namespace CompiladorRiquin
{
    public class ejecutarLexico
    {
        private List<Token> listaTokens;
        private List<Token> listaErrores;

        public ejecutarLexico()
        {
            this.listaTokens = new List<Token>();
            this.listaErrores = new List<Token>();

        }

        public void ejecutarIde(string texto)
        {
            ejecutar(texto, true);
        }

        public void ejecutarTerminal(string texto)
        {
            ejecutar(texto, false);
        }

        public string getListaTokens()
        {
            string text = "";
            for(int i = 0; i < listaTokens.Count; i++)
            {
                text += listaTokens[i].getToken() + " - ' " + listaTokens[i].getLexema() /*+ " - Linea: " + listaTokens[i].getFila().ToString() + " - Columna: " + listaTokens[i].getColumna().ToString()*/ + " '\n";
            }
            return text;
        }

        public List<Token> getTokens()
        {
            return this.listaTokens;
        }

        public string getListaErrores()
        {
            string text = "";
            for (int i = 0; i < listaErrores.Count; i++)
            {
                text += listaErrores[i].getToken() /*+ " - Linea: " + listaTokens[i].getFila().ToString() + " - Columna: " + listaTokens[i].getColumna().ToString()*/ + "\n";
            }
            return text;
        }


        private void ejecutar(string text, bool ide)
        {
            Dictionary<string, string> palabrasReservadas = new Dictionary<string, string>();
            Dictionary<string, string> caracteresEspeciales = new Dictionary<string, string>();
            palabrasReservadas.Add("main", "main");
            palabrasReservadas.Add("int", "int");
            palabrasReservadas.Add("float", "float");
            palabrasReservadas.Add("real", "real");
            palabrasReservadas.Add("if", "if");
            palabrasReservadas.Add("then", "then");
            palabrasReservadas.Add("else", "else");
            palabrasReservadas.Add("end", "end");
            palabrasReservadas.Add("do", "do");
            palabrasReservadas.Add("while", "while");
            palabrasReservadas.Add("cin", "cin");
            palabrasReservadas.Add("cout", "cout");
            palabrasReservadas.Add("boolean", "boolean");
            palabrasReservadas.Add("until", "until");
            //Caracteres especiales

            caracteresEspeciales.Add("{", "Llave abierta");
            caracteresEspeciales.Add("}", "Llave cerrada");
            caracteresEspeciales.Add("(", "Parentesis abierto");
            caracteresEspeciales.Add(")", "Parentesis Cerrado");
            caracteresEspeciales.Add("[", "Corchete abierto");
            caracteresEspeciales.Add("]", "Corchete cerrado");
            caracteresEspeciales.Add("*", "Multiplicacion");
            caracteresEspeciales.Add("/", "Diagonal");
            caracteresEspeciales.Add("%", "Porcentaje");
            caracteresEspeciales.Add(";", "Fin de sentencia");
            caracteresEspeciales.Add(",", "Coma");

            string textFile;

            Estados estado = Estados.Inicio;
            string texto = " ";
            if (ide)
            {
                texto = text + " ";
            }
            else
            {
                textFile = @text;
                if (File.Exists(textFile))
                {
                    // Read entire text file content in one string    
                    texto = File.ReadAllText(textFile);
                    Console.WriteLine(texto);
                }
                else
                {
                    Console.WriteLine("El archivo no existe");
                    return;
                }

            }



            char charConcatenar;

            string token = "";
            int i = 0, fila = 1, columna = 0;


            while (i < texto.Length)
            {
                charConcatenar = texto[i];
                switch (estado)
                {
                    case Estados.Inicio:
                        switch (charConcatenar)
                        {
                            case '\n':
                                fila++;
                                i++;
                                columna = 0;
                                break;
                            case '+':
                                token += charConcatenar;
                                estado = Estados.Mas;
                                i++; columna++;
                                break;
                            case '-':
                                token += charConcatenar;
                                estado = Estados.Menos;
                                i++; columna++;
                                break;
                            case '<':
                                token += charConcatenar;
                                estado = Estados.Menor;
                                i++; columna++;
                                break;
                            case '>':
                                token += charConcatenar;
                                estado = Estados.Mayor;
                                i++; columna++;
                                break;
                            case ':':
                                token += charConcatenar;
                                estado = Estados.DosPuntos;
                                i++; columna++;
                                break;
                            case '/':
                                token += charConcatenar;
                                estado = Estados.Diagonal;
                                i++; columna++;
                                break;
                            case '!':
                                token += charConcatenar;
                                estado = Estados.Diferente;
                                i++; columna++;
                                break;
                            case '=':
                                token += charConcatenar;
                                estado = Estados.Comparacion;
                                i++; columna++;
                                break;
                            case ' ':
                            case '\r':
                            case '\t':
                            case '\b':
                            case '\f':
                                estado = Estados.Inicio;
                                i++; columna++;
                                break;
                            default:
                                if (Char.IsLetter(charConcatenar))
                                {
                                    token += charConcatenar;
                                    estado = Estados.Id;
                                }
                                else if (Char.IsNumber(charConcatenar))
                                {
                                    token += charConcatenar;
                                    estado = Estados.Entero;
                                }
                                else
                                {
                                    token += charConcatenar;
                                    i++; columna++;
                                    if (caracteresEspeciales.ContainsKey(charConcatenar.ToString()))
                                    {
                                        listaTokens.Add(new Token(caracteresEspeciales[charConcatenar.ToString()], token, fila, columna, TipoToken.OTHER));
                                    }
                                    else
                                    {
                                        listaErrores.Add(new Token("Error. Caracter desconocido '" + token + "' - Linea: " + fila + " - Columna: " + columna, token, fila, columna, TipoToken.OTHER));
                                    }
                                    estado = Estados.Fin;
                                    break;
                                }
                                i++; columna++;
                                break;

                        }
                        break;

                    case Estados.Fin:
                        token = "";
                        estado = Estados.Inicio;
                        break;



                    case Estados.Id:
                        if (Char.IsLetterOrDigit(charConcatenar) || charConcatenar == '_')
                        {
                            token += charConcatenar;
                            i++; columna++;
                        }
                        else
                        {
                            if (palabrasReservadas.ContainsKey(token))
                            {
                                listaTokens.Add(new Token("Palabra reservada", token, fila, columna, TipoToken.ID_RESERVED));
                            }
                            else
                            {
                                listaTokens.Add(new Token("Identificador", token, fila, columna, TipoToken.ID));
                            }
                            estado = Estados.Fin;
                        }
                        break;

                    case Estados.Entero:
                        if (Char.IsNumber(charConcatenar))
                        {
                            token += charConcatenar;
                            i++; columna++;
                        }
                        else if (charConcatenar == '.')
                        {
                            token += charConcatenar;
                            estado = Estados.Punto;
                            i++; columna++;
                        }
                        else
                        {
                            //Hecho
                            listaTokens.Add(new Token("Numero", token, fila, columna, TipoToken.NUM));
                            estado = Estados.Fin;
                        }
                        break;

                    case Estados.Punto:
                        if (Char.IsNumber(charConcatenar))
                        {
                            token += charConcatenar;
                            estado = Estados.Flotante;
                            i++; columna++;
                        }
                        else
                        {
                            listaErrores.Add(new Token("Error. Se esperaba numero en linea: " + fila + " y columna: " + columna, token, fila, columna, TipoToken.OTHER));
                            estado = Estados.Fin;
                        }
                        break;

                    case Estados.Flotante:
                        if (Char.IsNumber(charConcatenar))
                        {
                            //concatenamos
                            token += charConcatenar;
                            i++; columna++;
                        }
                        else
                        {
                            //Hecho
                            listaTokens.Add(new Token("Flotante", token, fila, columna, TipoToken.NUM));
                            estado = Estados.Fin;
                        }
                        break;

                    case Estados.Mas:
                        if (charConcatenar == '+')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Incremento", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            listaTokens.Add(new Token("Mas", token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;

                    case Estados.Menos:
                        if (charConcatenar == '-')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Decremento", token, fila, columna,TipoToken.OTHER));
                        }
                        else
                        {
                            listaTokens.Add(new Token("Menos", token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;

                    case Estados.Mayor:
                        if (charConcatenar == '=')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Mayor o Igual", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            listaTokens.Add(new Token("Mayor", token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;

                    case Estados.Menor:
                        if (charConcatenar == '=')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Menor o Igual", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            listaTokens.Add(new Token("Menor", token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;
                    case Estados.DosPuntos:
                        if (charConcatenar == '=')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Asignación", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            //Error -> se esperaba =
                            listaErrores.Add(new Token("Error. Se esperaba '=' en linea: " + fila + " y columna: " + columna, token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;

                    case Estados.Diagonal:
                        if (charConcatenar == '/')
                        {
                            estado = Estados.CometarioLinea;
                            i++; columna++;
                        }
                        else if (charConcatenar == '*')
                        {
                            estado = Estados.AsteriscoAbrir;
                            i++; columna++;
                        }
                        else
                        {
                            listaTokens.Add(new Token("Division", token, fila, columna, TipoToken.OTHER));
                            estado = Estados.Fin;
                        }
                        break;

                    case Estados.CometarioLinea:
                        if (charConcatenar == '\n')
                        {
                            estado = Estados.Fin;
                        }
                        else
                        {
                            i++; columna++;
                        }
                        break;

                    case Estados.AsteriscoAbrir:
                        if (charConcatenar == '*')
                        {
                            estado = Estados.AsteriscoCerrar;
                        }
                        else if (charConcatenar == '\n')
                        {
                            fila++;
                            columna = -1;
                        }
                        i++; columna++;
                        break;

                    case Estados.AsteriscoCerrar:
                        if (charConcatenar == '/')
                        {
                            estado = Estados.Fin;
                            i++; columna++;
                        }
                        else if (charConcatenar == '*')
                        {
                            i++; columna++;
                        }
                        else
                        {
                            estado = Estados.AsteriscoAbrir;
                            i++; columna++;
                        }
                        break;

                    case Estados.Diferente:
                        if (charConcatenar == '=')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Diferente", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            listaErrores.Add(new Token("Error. Se esperaba '=' en linea: " + fila + " y columna: " + columna, token, fila, columna, TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;

                    case Estados.Comparacion:
                        if (charConcatenar == '=')
                        {
                            token += charConcatenar;
                            i++; columna++;
                            listaTokens.Add(new Token("Comparacion", token, fila, columna, TipoToken.OTHER));
                        }
                        else
                        {
                            listaErrores.Add(new Token("Error. Se esperaba '=' en linea: " + fila + " y columna: " + columna, token, fila, columna,TipoToken.OTHER));
                        }
                        estado = Estados.Fin;
                        break;
                }
            }

        }
    }

    public class Token
    {
        string token;
        string lexema;
        int fila;
        int columna;
        TipoToken tipo;

        public Token()
        {
            this.token = "";
            this.lexema = "";
            this.fila = 0;
            this.columna = 0;
            this.tipo = TipoToken.OTHER;
        }

        public Token(string t, string l, int f, int c, TipoToken tipo)
        {
            this.token = t;
            this.lexema = l;
            this.fila = f;
            this.columna = c;
            this.tipo = tipo;
        }

        public string getToken()
        {
            return this.token;
        }

        public string getLexema()
        {
            return this.lexema;
        }

        public int getFila()
        {
            return this.fila;
        }

        public int getColumna()
        {
            return this.columna;
        }

        public TipoToken getTipo()
        {
            return this.tipo;
        }


    }
}
