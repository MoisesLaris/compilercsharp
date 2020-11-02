using System;
using System.Collections.Generic;
namespace CompiladorRiquin

{
    public class Semantico
    {
        private nodo arbol;
        public static Dictionary<string, HashElement> tablaHash = new Dictionary<string, HashElement>();
        public static string erroresSemanticos = "";

        public Semantico(nodo arbol)
        {
            this.arbol = arbol;
        }


        public void mainSintactico()
        {
            listaDeclaraciones(this.arbol.hijos[0]); //Lista de declaraciones
            listaSentencias(this.arbol.hijos[1]);
        }



        //Lista sentencias

        public void listaSentencias(nodo arbol)
        {
            
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
            hashElement.valor = nodo.valor;
            hashElement.tipo = nodo.tipoNodo;
            hashElement.location = "";
            hashElement.lista = new List<int>();
            hashElement.lista.Add(nodo.linea);

            tablaHash.Add(nodo.nombre, hashElement);
            Console.WriteLine(tablaHash[nodo.nombre].nombre);
        }


        public static void fnResetRun()
        {
            erroresSemanticos = "";
            tablaHash.Clear();
        }

    }

    public class HashElement
    {
        public string nombre { get; set; }
        public string valor { get; set; }
        public string location { get; set; }
        public TipoNodo tipo { get; set; }
        public List<int> lista { get; set; }
    }
}
