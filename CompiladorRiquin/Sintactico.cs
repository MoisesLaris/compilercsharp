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
        undefined,
        error
    }
    public enum Tipo
    {
        sentencia,
        expresion,
        constante,
        operador,
        id,
    }

    public class nodo
    {
        public string nombre;
        public TipoNodo tipoNodo;
        public Tipo tipo;
<<<<<<< HEAD
=======
        public TipoNodo tipoNodoStatic;
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
        public float valor;
        public int linea;
        public List<nodo> hijos = new List<nodo>(); 

        public nodo()
        {
            this.nombre = "";
            this.valor = 0;
        }
<<<<<<< HEAD
        public nodo(string nombre, TipoNodo tipoNodo, Tipo tipo)
=======
        public nodo(string nombre, TipoNodo tipoNodo, Tipo tipo, TipoNodo tipoNodoStatic)
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.tipoNodo = tipoNodo;
<<<<<<< HEAD
            this.valor = 0;
        }
        public nodo(string nombre, TipoNodo tipoNodo, Tipo tipo, float valor)
=======
            this.tipoNodoStatic = tipoNodoStatic;
            this.valor = 0;
        }
        public nodo(string nombre, TipoNodo tipoNodo, Tipo tipo, TipoNodo tipoNodoStatic, float valor)
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
        {
            this.nombre = nombre;
            this.tipoNodo = tipoNodo;
            this.tipo = tipo;
<<<<<<< HEAD
=======
            this.tipoNodoStatic = tipoNodoStatic;
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
            this.valor = valor;
        }


        public nodo programa()
        {
            nodo temp = null;
            if(MainWindow.getToken().getLexema() == "main")
            {
                match("main");
<<<<<<< HEAD
                temp = new nodo("main", TipoNodo.exp, Tipo.expresion);
=======
                temp = new nodo("main", TipoNodo.exp, Tipo.expresion, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
            nodo temp = new nodo("declaracion", TipoNodo.exp, Tipo.expresion);
=======
            nodo temp = new nodo("declaracion", TipoNodo.exp, Tipo.expresion, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                    temp = createVariableType("int", TipoNodo.integer, Tipo.constante);
                    break;
                case "float":
                    temp = createVariableType("float", TipoNodo.float_number, Tipo.constante);
                    break;
                case "boolean":
                    temp = createVariableType("boolean", TipoNodo.boolean, Tipo.constante);
=======
                    temp = createVariableType("int", TipoNodo.integer, Tipo.constante, TipoNodo.exp);
                    break;
                case "float":
                    temp = createVariableType("float", TipoNodo.float_number, Tipo.constante, TipoNodo.exp);
                    break;
                case "boolean":
                    temp = createVariableType("boolean", TipoNodo.boolean, Tipo.constante, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                    break;
            
            }
            return temp;
        }

<<<<<<< HEAD
        public nodo createVariableType(string variable, TipoNodo tipo, Tipo tipo1)
        {
            nodo temp = new nodo(variable, tipo, tipo1);
=======
        public nodo createVariableType(string variable, TipoNodo tipo, Tipo tipo1, TipoNodo tipoStatic)
        {
            nodo temp = new nodo(variable, tipo, tipo1, tipoStatic);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
            nodo temp = new nodo("sentencias",TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("sentencias",TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
            nodo temp = new nodo("if", TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("if", TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp, Tipo.sentencia);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                match(":=");
                temp.hijos.Add(izq);
                temp.hijos.Add(exp());
                match(";");
                //temp.nodos[0] = izq;
                //temp.nodos[1] = exp();     
            }
            else if (MainWindow.getToken().getLexema() == "++" || MainWindow.getToken().getLexema() == "--")
            {
<<<<<<< HEAD
                temp = new nodo(":=", TipoNodo.exp, Tipo.sentencia);
=======
                temp = new nodo(":=", TipoNodo.exp, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin

                string s;
                if (MainWindow.getToken().getLexema() == "++")
                {
                    s = "+";
                }
                else
                {
                    s = "-";
                }

<<<<<<< HEAD
                nodo mas_menos = new nodo(s, TipoNodo.exp, Tipo.expresion);
                mas_menos.hijos.Add(new nodo(izq.nombre, TipoNodo.id, Tipo.constante));
                mas_menos.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, 1));
=======
                nodo mas_menos = new nodo(s, TipoNodo.exp, Tipo.expresion, TipoNodo.exp);
                mas_menos.hijos.Add(new nodo(izq.nombre, TipoNodo.id, Tipo.id, TipoNodo.exp));
                mas_menos.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, TipoNodo.exp, 1));
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin

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
<<<<<<< HEAD
            nodo temp = new nodo("cin", TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("cin", TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
            nodo temp = new nodo("cout",TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("cout",TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
            match("cout");
            temp.hijos.Add(exp());
            match(";");
            //temp.nodos[0] = exp();
            return temp;
        }

        public nodo repeticion()
        {
<<<<<<< HEAD
            nodo temp = new nodo("do", TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("do", TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
            nodo temp = new nodo("while", TipoNodo.sent, Tipo.sentencia);
=======
            nodo temp = new nodo("while", TipoNodo.sent, Tipo.sentencia, TipoNodo.sent);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                nuevo = new nodo(MainWindow.getToken().getLexema(),TipoNodo.exp, Tipo.operador);
=======
                nuevo = new nodo(MainWindow.getToken().getLexema(),TipoNodo.exp, Tipo.operador, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
=======
                        nuevo.tipoNodoStatic = TipoNodo.exp;
                        nuevo.tipo = Tipo.operador;
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
=======
                        nuevo.tipoNodoStatic = TipoNodo.exp;
                        nuevo.tipo = Tipo.operador;
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                        match("-");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = term();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(term());
                        temp = nuevo;
                        break;
                    case "++":
<<<<<<< HEAD
                        nuevo = new nodo(":=",TipoNodo.exp, Tipo.expresion);
                        nodo mas = new nodo("+", TipoNodo.exp, Tipo.operador);
                        mas.hijos.Add(temp);
                        mas.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, 1));
=======
                        nuevo = new nodo(":=",TipoNodo.exp, Tipo.sentencia, TipoNodo.sent);
                        nodo mas = new nodo("+", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
                        mas.hijos.Add(temp);
                        mas.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, TipoNodo.exp, 1));
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin

                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(mas);
                        match("++");
                        temp = nuevo;
                        break;
                    case "--":
<<<<<<< HEAD
                        nuevo = new nodo(":=", TipoNodo.exp, Tipo.expresion);
                        nodo menos = new nodo("-", TipoNodo.exp, Tipo.operador);
                        menos.hijos.Add(temp);
                        menos.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, 1));
=======
                        nuevo = new nodo(":=", TipoNodo.exp, Tipo.sentencia, TipoNodo.sent);
                        nodo menos = new nodo("-", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
                        menos.hijos.Add(temp);
                        menos.hijos.Add(new nodo("1", TipoNodo.integer, Tipo.constante, TipoNodo.exp, 1));
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin

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
<<<<<<< HEAD
                        nuevo = new nodo("*", TipoNodo.exp, Tipo.operador);
=======
                        nuevo = new nodo("*", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                        match("*");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = fac();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(fac());
                        temp = nuevo;
                        break;
                    case "/":
<<<<<<< HEAD
                        nuevo = new nodo("/", TipoNodo.exp, Tipo.operador);
=======
                        nuevo = new nodo("/", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                        match("/");
                        //nuevo.nodos[0] = temp;
                        //nuevo.nodos[1] = fac();
                        nuevo.hijos.Add(temp);
                        nuevo.hijos.Add(fac());
                        temp = nuevo;
                        break;
                    case "%":
<<<<<<< HEAD
                        nuevo = new nodo("%", TipoNodo.exp, Tipo.operador);
=======
                        nuevo = new nodo("%", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                    nuevo = new nodo("^", TipoNodo.exp, Tipo.operador);
=======
                    nuevo = new nodo("^", TipoNodo.exp, Tipo.operador, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.integer, Tipo.constante);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.integer, Tipo.constante, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                temp.valor = integer;
                MainWindow.iterator++;
            }
            else if (float.TryParse(MainWindow.getToken().getLexema(), out real))
            {
                Console.WriteLine("Numero real -> " + real);
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.float_number, Tipo.constante);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.float_number, Tipo.constante, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
                temp.valor = real;
                MainWindow.iterator++;
            }
            else if (MainWindow.getToken().getTipo() == TipoToken.ID)
            {
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id, Tipo.id);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id, Tipo.id, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id, Tipo.id);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.id, Tipo.id, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
<<<<<<< HEAD
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp, Tipo.id);
=======
                temp = new nodo(MainWindow.getToken().getLexema(), TipoNodo.exp, Tipo.id, TipoNodo.exp);
>>>>>>> 21d5f21... Initial check-in of module CompiladorRiquin
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
