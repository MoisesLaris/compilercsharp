using System;
using System.Collections.Generic;

namespace CompiladorRiquin
{
    public enum Sentencia
    {
        IF,
        REPEAT,
        ASSIGN,
        READ,
        WRITE
    }

    public enum Expresion
    {
        NUM,
        FLOAT,
        ID,
        BOOLEAN
    }

    public enum TipoNodo
    {
        sent,
        exp,
        integer,
        float_number,
        boolean,
        id,


    }

    public class nodo
    {
        public string nombre;
        public TipoNodo tipoNodo;
        public float valor;
        public int linea;
        public List<nodo> hijos = new List<nodo>(); 

        public nodo()
        {
            this.nombre = "";
        }
        public nodo(string nombre, TipoNodo tipoNodo)
        {
            this.nombre = nombre;
            this.tipoNodo = tipoNodo;
        }
        public nodo(string nombre, TipoNodo tipoNodo, float valor)
        {
            this.nombre = nombre;
            this.tipoNodo = tipoNodo;
            this.valor = valor;
        }


        public nodo programa()
        {
            nodo temp = null;
            if(MainWindow.getToken().getLexema() == "main")
            {
                match("main");
                temp = new nodo("main", TipoNodo.exp);
                match("{");
                //temp.nodos[0] = lista_declaracion();
                temp.hijos.Add(lista_declaracion());
                //temp.nodos[1] = lista_sentencia();
                temp.hijos.Add(lista_sentencia());
                match("}");
            }
            else
            {
                Console.WriteLine("error");
            }
            return temp;  
        }


        public nodo lista_declaracion()
        {
            nodo temp = new nodo("declaracion", TipoNodo.exp);
            nodo q = declaracion();
            if(q != null)
                temp.hijos.Add(q);
            while(q!=null)
            {
                q = declaracion();
                if (q != null)
                {
                    temp.hijos.Add(q);
                }
                else
                {
                    break;
                }
            }
            
            return temp;
        }


        public nodo declaracion()
        {

            nodo temp = tipoVariable();
            if (temp == null)
            {
                return temp;
            }

            nodo q;
            q = finID();
            if (q != null)
            {
                temp.hijos.Add(q);
            }
            else
            {
                return temp;
            }

            if (MainWindow.getToken().getLexema() == ",")
            {
                while(MainWindow.getToken().getLexema() == ",")
                {
                    match(",");
                    q = finID();
                    if (q != null)
                    {
                        temp.hijos.Add(q);
                    }
                    else
                    {
                        MainWindow.erroresSintactico += "Se esperaba identificador  Linea: " + MainWindow.getTokenAnterior().getFila() + "  Columna: " + MainWindow.getTokenAnterior().getColumna() + "\n";
                        return temp;
                    }
                }
            }
            if(MainWindow.getToken().getLexema() == ";")
            {
                match(";");
            }
            else
            {
                MainWindow.erroresSintactico += "Se esperaba ;  Linea: " + MainWindow.getTokenAnterior().getFila() + "  Columna: " + MainWindow.getTokenAnterior().getColumna() + "\n";
            }
         

            return temp;
        }




        //match(",");
        //while (true)
        //{

        //    q = finID();
        //    if (q == null && MainWindow.getToken().getLexema() == ";") {
        //        break;
        //    }

        //    if (q != null)
        //    {
        //        temp.hijos.Add(q);
        //    }
        //    if(q!=null && (MainWindow.getToken().getTipo() != TipoToken.ID && MainWindow.getToken().getLexema() != "," && MainWindow.getToken().getLexema() != ";"))
        //    {
        //        MainWindow.erroresSintactico += "Se esperaba ;  Linea: " + MainWindow.getTokenAnterior().getFila() + "  Columna: " + MainWindow.getTokenAnterior().getColumna() + "\n";
        //        break;
        //    }
        //    if(MainWindow.getToken().getLexema() == ",")
        //    {
        //        match(",");
        //    }
        //}



        public nodo tipoVariable()
        {
            nodo temp = null;
            switch (MainWindow.getToken().getLexema())
            {
                case "int":
                    temp = createVariableType("int", TipoNodo.integer);
                    break;
                case "float":
                    temp = createVariableType("float", TipoNodo.float_number);
                    break;
                case "boolean":
                    temp = createVariableType("boolean", TipoNodo.boolean);
                    break;
            
            }
            return temp;
        }

        public nodo createVariableType(string variable, TipoNodo tipo)
        {
            nodo temp = new nodo(variable, tipo);
            match(variable);
            return temp;
        }

        public nodo lista_variables()
        {
            
            nodo temp = finID();
            nodo p = temp;
            while(MainWindow.getToken().getLexema() != ";")
            {
                nodo q;
                match(",");
                q = finID();

                if(q == null)
                {
                    break;
                }
            }
            return temp;
        }


        public nodo lista_sentencia()
        {
            nodo temp = new nodo("sentencias",TipoNodo.sent);
            nodo s = new nodo();
            temp.hijos.Add(sentencia());
            while(MainWindow.getToken().getLexema() != "end" && MainWindow.getToken().getLexema() != "else" && MainWindow.getToken().getLexema() != "until" && MainWindow.getToken().getLexema() != "}" && MainWindow.iterator < MainWindow.listaTokens.Count)
            {
                s = sentencia();
                if (s != null)
                {
                    temp.hijos.Add(s);
                }
                else
                {
                    MainWindow.iterator++;
                }
                
            }
            return temp;
        }

        public nodo sentencia()
        {
            nodo temp = null;
            Token token = MainWindow.getToken();
            switch (token.getLexema())
            {
                case "if":
                    temp = condicion();
                    return temp;
                case "cin":
                    temp = cin();
                    return temp;
                case "cout":
                    temp = cout();
                    return temp;
                case "do":
                    temp = repeticion();
                    return temp;
                case "while":
                    temp = iteracion();
                    return temp;
            }
            if(token.getTipo() == TipoToken.ID)
            {
                temp = asignar();
                return temp;
            }
            else
            {
                MainWindow.erroresSintactico += "Valor inesperado -> " + MainWindow.getToken().getLexema() + "  Linea: " + MainWindow.getTokenAnterior().getFila() + "  Columna: " + MainWindow.getTokenAnterior().getColumna() + "\n";
                return temp;
            }
            
        }


        public nodo condicion()
        {
            nodo temp = new nodo("if", TipoNodo.sent);
            match("if");

            //temp.nodos[0] = exp();
            match("(");
            temp.hijos.Add(exp());
            match(")");
            match("then");

            //temp.nodos[1] = lista_sentencia();
            temp.hijos.Add(lista_sentencia());
            Console.WriteLine(MainWindow.getToken().getLexema());
            if (MainWindow.getToken().getLexema() == "else")
            {
                match("else");
                //temp.nodos[2] = lista_sentencia();
                temp.hijos.Add(lista_sentencia());
            }
            match("end");
            match(";");
            return temp;
        }

        
        public nodo asignar()
        {
            nodo temp = null;
            nodo izq = finID();
           
            if (MainWindow.getToken().getLexema() == ":=")
            {
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp);
                match(":=");
                temp.hijos.Add(izq);
                temp.hijos.Add(exp());
                match(";");
                //temp.nodos[0] = izq;
                //temp.nodos[1] = exp();     
            }
            else if (MainWindow.getToken().getLexema() == "++" || MainWindow.getToken().getLexema() == "--")
            {
                temp = new nodo(":=", TipoNodo.exp);

                string s = "";
                if (MainWindow.getToken().getLexema() == "++")
                {
                    s = "+";
                }
                else
                {
                    s = "-";
                }

                nodo mas_menos = new nodo(s, TipoNodo.exp);
                mas_menos.hijos.Add(izq);
                mas_menos.hijos.Add(new nodo("1", TipoNodo.integer,1));

                temp.hijos.Add(izq);
                temp.hijos.Add(mas_menos);

                match(MainWindow.getToken().getLexema());
                match(";");
            }
            else
            {
                error_ant("asignacion, incremento o decremento");
            }

            return temp;
        }



        public nodo cin()
        {
            nodo temp = new nodo("cin", TipoNodo.sent);
            match("cin");
            if(MainWindow.getToken().getTipo() == TipoToken.ID)
            {
                temp.hijos.Add(fin());
                //temp.nodos[0] = fin();
            }
            else
            {
                error_ant("identificador");
                return temp;
            }
            match(";");
            return temp;
        }

        public nodo cout()
        {
            nodo temp = new nodo("cout",TipoNodo.sent);
            match("cout");
            temp.hijos.Add(exp());
            match(";");
            //temp.nodos[0] = exp();
            return temp;
        }

        public nodo repeticion()
        {
            nodo temp = new nodo("do", TipoNodo.sent);
            match("do");
            //temp.nodos[0] = lista_sentencia();
            temp.hijos.Add(lista_sentencia());
            match("until");
            match("(");
            //temp.nodos[1] = exp();
            temp.hijos.Add(exp());
            match(")");
            match(";");
            return temp;
        }

        public nodo iteracion()
        {
            nodo temp = new nodo("while", TipoNodo.sent);
            match("while");
            match("(");
            temp.hijos.Add(exp());
            match(")");
            temp.hijos.Add(bloque());
            return temp;
        }

        public nodo bloque()
        {
            nodo temp = null;
            match("{");
            temp = lista_sentencia();
            match("}");
            return temp;
        }


        public nodo exp()
        {
            nodo temp, nuevo;
            temp = exp_simple();
            while(MainWindow.getToken().getLexema() == "<=" || MainWindow.getToken().getLexema() == "<" || MainWindow.getToken().getLexema() == ">=" || MainWindow.getToken().getLexema() == ">" || MainWindow.getToken().getLexema() == "==" || MainWindow.getToken().getLexema() == "!=")
            {
                nuevo = new nodo(MainWindow.getToken().getLexema(),TipoNodo.exp);
                match(MainWindow.getToken().getLexema());
                //nuevo.nodos[0] = temp;
                //nuevo.nodos[1] = exp_simple();
                nuevo.hijos.Add(temp);
                nuevo.hijos.Add(exp_simple());
                temp = nuevo;
            }
            return temp;
        }


        public nodo exp_simple()
        {
            
            nodo temp, nuevo;
            temp = term();
            Console.WriteLine(MainWindow.getToken().getLexema());
            while (MainWindow.getToken().getLexema() == "+" || MainWindow.getToken().getLexema() == "-" || MainWindow.getToken().getLexema() == "++" || MainWindow.getToken().getLexema() == "--")
            {
                Console.WriteLine(MainWindow.getToken().getLexema());

                switch (MainWindow.getToken().getLexema())
                {
                    case "+":
                        nuevo = new nodo();
                        nuevo.nombre = MainWindow.getToken().getLexema();
                        match("+");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = term();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(term());
                        temp = nuevo;
                        break;

                    case "-":
                        nuevo = new nodo();
                        nuevo.nombre = MainWindow.getToken().getLexema();
                        match("-");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = term();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(term());
                        temp = nuevo;
                        break;
                    case "++":
                        nuevo = new nodo(":=",TipoNodo.exp);
                        nodo mas = new nodo("+", TipoNodo.exp);
                        mas.hijos.Add(temp);
                        mas.hijos.Add(new nodo("1", TipoNodo.integer, 1));

                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(mas);
                        match("++");
                        temp = nuevo;
                        break;
                    case "--":
                        nuevo = new nodo(":=", TipoNodo.exp);
                        nodo menos = new nodo("-", TipoNodo.exp);
                        menos.hijos.Add(temp);
                        menos.hijos.Add(new nodo("1", TipoNodo.integer, 1));

                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(menos);
                        match("++");
                        temp = nuevo;
                        break;
                }
            }
            return temp;
        }

        public nodo term()
        {

            nodo temp, nuevo;
            temp = fac();
            while (MainWindow.getToken().getLexema() == "/" || MainWindow.getToken().getLexema() == "*" || MainWindow.getToken().getLexema() == "%")
            {
                switch (MainWindow.getToken().getLexema())
                {
                    case "*":
                        nuevo = new nodo("*", TipoNodo.exp);
                        match("*");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = fac();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(fac());
                        temp = nuevo;
                        break;
                    case "/":
                        nuevo = new nodo("/", TipoNodo.exp);
                        match("/");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = fac();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(fac());
                        temp = nuevo;
                        break;
                    case "%":
                        nuevo = new nodo("%", TipoNodo.exp);
                        match("%");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = fac();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(fac());
                        temp = nuevo;
                        break;
                }
            }

            return temp;
        }

        public nodo fac()
        {

            nodo nuevo, temp;
            temp = fin();
            while (MainWindow.getToken().getLexema() == "^")
            {
                if (MainWindow.getToken().getLexema() == "^")
                {
                    nuevo = new nodo("^", TipoNodo.exp);
                    match("^");
                    nuevo.hijos.Add(temp);
                    nuevo.hijos.Add(fin());
                    //nuevo.nodos[0] = temp;
                    //nuevo.nodos[1] = fin();
                    temp = nuevo;
                }
            }
            return temp;
        }

        public nodo fin()
        {

            float real;
            int integer;
            nodo temp = null;
            if(MainWindow.getToken().getLexema() == "(")
            {
                match("(");
                temp = exp();
                match(")");
            }
            else if (int.TryParse(MainWindow.getToken().getLexema(), out integer))
            {
                Console.WriteLine("Numero entero -> " + integer);
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.integer);
                temp.valor = integer;
                MainWindow.iterator++;
            }
            else if (float.TryParse(MainWindow.getToken().getLexema(), out real))
            {
                Console.WriteLine("Numero real -> " + real);
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.float_number);
                temp.valor = real;
                MainWindow.iterator++;
            }
            else if (MainWindow.getToken().getTipo() == TipoToken.ID)
            {
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id);
                temp.linea = MainWindow.getToken().getFila();
                MainWindow.iterator++;
            }
            else
            {
                Console.WriteLine("error ->" + MainWindow.getToken().getLexema());
            }
            return temp;
           
        }

        public nodo finID()
        {
            nodo temp = null;
            if(MainWindow.getToken().getTipo() == TipoToken.ID)
            {
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id);
                temp.linea = MainWindow.getToken().getFila();
                MainWindow.iterator++;
            }
            return temp;
        }

        public nodo finID_RESERVED()
        {
            nodo temp = null;
            if (MainWindow.getToken().getTipo() == TipoToken.ID_RESERVED)
            {
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp);
                MainWindow.iterator++;
            }
                return temp;
        }

        public void match(string token)
        {
            if(token == MainWindow.getToken().getLexema())
            {
                MainWindow.iterator++;
            }
            else
            {
                error_ant(token);
            }
            
        }

        public void error_ant(string expected)
        {
            MainWindow.erroresSintactico += "Se esperaba "+ expected + "  Linea: " + MainWindow.getTokenAnterior().getFila() + "  Columna: " + MainWindow.getTokenAnterior().getColumna() + "\n";
        }

        

    }

}
