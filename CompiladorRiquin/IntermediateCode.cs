using System;
using System.IO;
using System.Threading;

namespace CompiladorRiquin
{
    public class IntermediateCode
    {
        private nodo arbol;
        public static string codigoIntermedio = "";
        public static string erroresCodigoIntermedio = "";

        //Variables a definir
        public static int emitLoc = 0;
        public static int highEmitLoc = 0;
        public int pc = 7;

        //DEFINE
        public int mp = 6;
        public int ac = 0;
        public int ac1 = 1;
        public int gp = 5;

        public static int tmpOffset = 0;

        public IntermediateCode(nodo arbol)
        {
            this.arbol = arbol;
        }

        public void ejectIntermediateCode()
        {
            codigoIntermedio = "";

            if(Semantico.erroresSemanticos != "")
            {
                erroresCodigoIntermedio = "Errores semanticos pendientes";
                return;
            }
            emitLoc = 0;
            highEmitLoc = 0;
            codeIntermediate_process(this.arbol.hijos[1]);


            var path = @"./data.txt";

            File.WriteAllText(path, codigoIntermedio);

        }


        public void codeIntermediate_process(nodo arbol)
        {
            emitRM("LD", mp, 0, ac, "Load max address");
            emitRM("ST", ac, 0, ac, "Clear location 0");
            foreach (var hijo in arbol.hijos)
            {
                cGen(hijo);
            }
            emitRO("HALT", 0, 0, 0, "");
        }

        public void cGen(nodo arbol)
        {
            if(arbol != null)
            {
                switch (arbol.tipoNodoStatic)
                {
                    case TipoNodo.exp:
                        genExp(arbol);
                        break;
                    case TipoNodo.sent:
                        genStmt(arbol);
                        break;
                }
            }
            
        }

        public void genExp(nodo arbol)
        {
            int loc;
            switch (arbol.tipo)
            {
                case Tipo.id:
                    loc = st_lookup(arbol.nombre);
                    emitRM("LD", ac, loc, gp, "Load id value");
                    break;
                case Tipo.constante:
                    emitRM("LDC", ac, (int)arbol.valor, 0, "load const");
                    break;
                default:
                    if (arbol.hijos.Count == 2)
                    {
                        cGen(arbol.hijos[0]);
                        emitRM("ST", ac, tmpOffset--, mp, "Op: push left");
                        cGen(arbol.hijos[1]);
                        emitRM("LD", ac1, ++tmpOffset, mp, "Op: load left");
                        switch (arbol.nombre)
                        {
                            case "+":
                                emitRO("ADD", ac, ac1, ac, "op +");
                                break;
                            case "-":
                                emitRO("SUB", ac, ac1, ac, "op -");
                                break;
                            case "*":
                                emitRO("MUL", ac, ac1, ac, "op *");
                                break;
                            case "/":
                                emitRO("DIV", ac, ac1, ac, "op /");
                                break;
                            case "^":
                                emitRO("POT", ac, ac1, ac, "op ^");
                                break;
                            case "%":
                                emitRO("MOD", ac, ac1, ac, "op %");
                                break;
                            case "<":
                                emitRO("SUB", ac, ac1, ac, "op <");
                                emitRM("JLT", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                            case "==":
                                emitRO("SUB", ac, ac1, ac, "op ==");
                                emitRM("JEQ", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                            case "<=":
                                emitRO("SUB", ac, ac1, ac, "op <=");
                                emitRM("JLE", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                            case ">=":
                                emitRO("SUB", ac, ac1, ac, "op >=");
                                emitRM("JGE", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                            case ">":
                                emitRO("SUB", ac, ac1, ac, "op >");
                                emitRM("JGT", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                            case "!=":
                                emitRO("SUB", ac, ac1, ac, "op !=");
                                emitRM("JNE", ac, 2, pc, "br if true");
                                emitRM("LDC", ac, 0, ac, "false case");
                                emitRM("LDA", pc, 1, pc, "unconditional jmp");
                                emitRM("LDC", ac, 1, ac, "true case");
                                break;
                        }
                    }
                    break;
            }
        }

        public void genStmt(nodo arbol)
        {
            int savedLoc1, savedLoc2, currentLoc;
            int loc;
            if (arbol != null)
            {
                switch (arbol.nombre)
                {
                    case ":=":
                        cGen(arbol.hijos[1]);
                        loc = st_lookup(arbol.hijos[0].nombre);
                        emitRM("ST", ac, loc, gp, "Asigna valor guardado");
                        break;
                    case "cin":
                        emitRO("IN", ac, 0, 0, "Read integer value");
                        loc = st_lookup(arbol.hijos[0].nombre);
                        emitRM("ST", ac, loc, gp, "read: store value");
                        break;
                    case "cout":
                        cGen(arbol.hijos[0]);
                        emitRO("OUT", ac, 0, 0, "Write ac");
                        break;
                    case "if":
                        cGen(arbol.hijos[0]);
                        savedLoc1 = emitSkip(1);

                        foreach (var rama in arbol.hijos[1].hijos)
                        {
                            cGen(rama);
                        }

                        savedLoc2 = emitSkip(1);
                        currentLoc = emitSkip(0);
                        emitBackup(savedLoc1);
                        emitRM_Abs("JEQ", ac, currentLoc, "if: jmp to else");
                        emitRestore();
                        if(arbol.hijos.Count > 2)
                        {
                            foreach (var rama in arbol.hijos[2].hijos)
                            {
                                cGen(rama);
                            }
                        }
                        currentLoc = emitSkip(0);
                        emitBackup(savedLoc2);
                        emitRM_Abs("LDA", pc, currentLoc, "jmp to end");
                        emitRestore();
                        break;
                    case "do":
                        savedLoc1 = emitSkip(0);
                        for(int i = 0; i < arbol.hijos[0].hijos.Count; i++)
                        {
                            cGen(arbol.hijos[0].hijos[i]);
                        }
                        cGen(arbol.hijos[1]);
                        emitRM_Abs("JEQ", ac, savedLoc1, "repeat: jmp back to body");
                        break;

                }
            }
        }



        public void emitRO(string op, int r, int s, int t, string comentario)
        {
            codigoIntermedio += emitLoc++.ToString() + ": " + op.ToString() + "  " + r.ToString() + "," + s.ToString() + "," + t.ToString() + "\n";
            if(highEmitLoc < emitLoc)
            {
                highEmitLoc = emitLoc;
            }
        }

        public void emitRM(string op, int r, int d, int s, string comentario)
        {
            codigoIntermedio += emitLoc++.ToString() + ": " + op.ToString() + "  " + r.ToString() + "," + d.ToString() + "(" + s.ToString() + ")" + "\n";
            if (highEmitLoc < emitLoc)
            {
                highEmitLoc = emitLoc;
            }
        }

        public int emitSkip(int howMany)
        {
            int i = emitLoc;
            emitLoc += howMany;
            if(highEmitLoc < emitLoc)
            {
                highEmitLoc = emitLoc;
            }
            return i;
        }

        public void emitBackup(int loc)
        {
            emitLoc = loc;
        }

        public void emitRestore()
        {
            emitLoc = highEmitLoc;
        }

        public void emitRM_Abs(string op, int r, int a, string comentario)
        {
            codigoIntermedio += emitLoc.ToString() + ": " + op + "  " + r.ToString() + "," + (a - (emitLoc + 1)).ToString() + "(" + pc + ")\n";
            ++emitLoc;
            if(highEmitLoc < emitLoc)
            {
                highEmitLoc = emitLoc;
            }
        }
       


        public int st_lookup(string nombre)
        {
            int x = -1;
            if (Semantico.tablaHash.ContainsKey(nombre))
            {
                int.TryParse(Semantico.tablaHash[nombre].location, out x);
            }
            return x;
            
        }


    }
}
