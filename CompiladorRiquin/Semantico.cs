using System;
using System.Collections.Generic;
namespace CompiladorRiquin

{
    public class Semantico
    {
        private nodo arbol;
        public static Dictionary<string, HashElement> tablaHash = new Dictionary<string, HashElement>();
        public static string erroresSemanticos = "";
        public static int location = 0;

        public Semantico(nodo arbol)
        {
            this.arbol = arbol;
        }

        public nodo getArbol()
        {
            return this.arbol;
        }


        public void mainSintactico()
        {
            listaDeclaraciones(this.arbol.hijos[0]); //Lista de declaraciones
            listaSentencias(this.arbol.hijos[1]);
        }



        //Lista sentencias

        public void listaSentencias(nodo arbol)
        {
            arbol.hijos.ForEach(nodo =>
            {
                switch (nodo.nombre)
                {
                    case ":=": //Asignación
                        nodo.hijos[0].valor = fnCalcularValor(nodo.hijos[1]);
                        fnUpdateHashTable(nodo.hijos[0], nodo.hijos[1].tipoNodo);
                        break;
                }
            });
            
        }

        public float fnCalcularValor(nodo arbol)
        {
            switch (arbol.nombre)
            {
                case "-":
                    arbol.valor = fnCalcularValor(arbol.hijos[0]) - fnCalcularValor(arbol.hijos[1]);
                    break;
                case "+":
                    arbol.valor = fnCalcularValor(arbol.hijos[0]) + fnCalcularValor(arbol.hijos[1]);
                    break;
                case "*":
                    arbol.valor = fnCalcularValor(arbol.hijos[0]) * fnCalcularValor(arbol.hijos[1]);
                    break;
                case "/":
                    arbol.valor = fnCalcularValor(arbol.hijos[0]) / fnCalcularValor(arbol.hijos[1]);
                    break;
            }
            Console.WriteLine(arbol.nombre + " -> " + arbol.linea.ToString());

            if (arbol.tipoNodo == TipoNodo.id) //un numero o un id son el fin de un nodo
            {
                if (tablaHash.ContainsKey(arbol.nombre))
                {
                    Console.WriteLine(arbol.nombre + " -> " + arbol.linea.ToString());
                    tablaHash[arbol.nombre].lista.Add(arbol.linea);
                    arbol.valor = tablaHash[arbol.nombre].valor;
                    arbol.tipoNodo = tablaHash[arbol.nombre].tipo;
                }
                else
                {
                    erroresSemanticos += "Linea " + arbol.linea + ": La variable " + arbol.nombre + " no ha sido declarada\n";
                    return 0; //Significa que la variable x se esta usando cuando NO ESTA DECLARADA
                }
            }else if(arbol.hijos.Count > 0)
            {
                arbol.tipoNodo = fnCheckNode(arbol);
            }



            return arbol.valor;
        }

        public TipoNodo fnCheckNode(nodo raiz)
        {
            if(raiz.hijos[0].tipoNodo != raiz.hijos[0].tipoNodo)
            {
                if((raiz.hijos[0].tipoNodo == TipoNodo.boolean || raiz.hijos[0].tipoNodo == TipoNodo.boolean))
                {
                    erroresSemanticos += "Linea: " + raiz.linea +" .No se puede ejecutar la operacion " + raiz.nombre + " con esos tipos de datos.\n";
                }
                return TipoNodo.float_number;
            }
            else
            {
                return TipoNodo.integer;
            }
        }

        public void fnUpdateHashTable(nodo arbol, TipoNodo tipo)
        {
            if (tablaHash.ContainsKey(arbol.nombre)) {
                arbol.tipoNodo = tablaHash[arbol.nombre].tipo;
                tablaHash[arbol.nombre].lista.Add(arbol.linea);
                if (arbol.tipoNodo == tipo || (arbol.tipoNodo == TipoNodo.float_number && tipo == TipoNodo.integer)){
                    Console.WriteLine("Variable: " + arbol.nombre + " Valor: " + arbol.valor);
                    tablaHash[arbol.nombre].valor = arbol.valor;
                    return;
                }
                else
                {
                    string desde, hasta;
                    if (tipo == TipoNodo.integer)
                        hasta = "Int";
                    else if (tipo == TipoNodo.float_number)
                        hasta = "Float";
                    else
                        hasta = "Bool";

                    if (arbol.tipoNodo == TipoNodo.integer)
                        desde = "Int";
                    else if (arbol.tipoNodo == TipoNodo.float_number)
                        desde = "Float";
                    else
                        desde = "Bool";


                    erroresSemanticos += "Linea: " + arbol.linea + ". No se puede asignar un valor del tipo " + hasta + " a uno del tipo " + desde + "\n";
                }
            }
            else
            {
                erroresSemanticos += "La variable " + arbol.nombre + " no ha sido declarada\n";
            }

        }



        //Lista declaraciones

        public void listaDeclaraciones(nodo arbol)
        {
            Console.WriteLine(arbol.hijos.Count);

            for(int i = 0; i < arbol.hijos.Count; i++)
            {
                switch (arbol.hijos[i].tipoNodo)
                {
                    case TipoNodo.integer:
                        initializeHashTable(arbol.hijos[i], TipoNodo.integer);
                        break;
                    case TipoNodo.float_number:
                        initializeHashTable(arbol.hijos[i], TipoNodo.float_number);
                        break;
                    case TipoNodo.boolean:
                        initializeHashTable(arbol.hijos[i], TipoNodo.boolean);
                        break;
                }
            }
        }

        public void initializeHashTable(nodo arbol, TipoNodo tipo)
        {
            for(int i = 0; i<arbol.hijos.Count; i++)
            {
                arbol.hijos[i].tipoNodo = tipo;
                pushIntoHashTable(arbol.hijos[i]);
            }
        }


        public void pushIntoHashTable(nodo nodo)
        {
            if (tablaHash.ContainsKey(nodo.nombre))
            {
                erroresSemanticos += "Variable: " + nodo.nombre + " duplicada en linea:" + nodo.linea + "\n";
                return;
            }
            HashElement hashElement = new HashElement();
            hashElement.nombre = nodo.nombre;
            hashElement.valor = 0;
            hashElement.tipo = nodo.tipoNodo;
            hashElement.location = location.ToString();
            hashElement.lista = new List<int>();
            hashElement.lista.Add(nodo.linea);

            tablaHash.Add(nodo.nombre, hashElement);
            Console.WriteLine(tablaHash[nodo.nombre].nombre);
            location++;
        }


        public static void fnResetRun()
        {
            location = 0;
            erroresSemanticos = "";
            tablaHash.Clear();
        }

    }

    public class HashElement
    {
        public string nombre { get; set; }
        public float valor { get; set; }
        public string location { get; set; }
        public TipoNodo tipo { get; set; }
        public List<int> lista { get; set; }
    }
}
