using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Gtk;
using CompiladorRiquin;

public partial class MainWindow : Gtk.Window
{
    public int lines = 0;

    public int caracteresActuales = 0;  //Caracteres de referencia para guardar archivo.
    public int caracteresEscritos = 0;  //Si es mayor o menor a los caracteres actuales entonces significa que está editando

    public string currentFile = "";

    public bool changed = false; //Cuando se detectan cambios deja o no guardar.

    public string[] PalabrasReservadas = new string[] { "int", "float", "real", "boolean", "if", "else", "then", "while", "until", "main", "end", "do", "cin", "cout" };

    

    //Variables para que nuestra clase nodo pueda acceder...
    public static string erroresSintactico = "";
    public static List<Token> listaTokens = new List<Token>();
    public static int iterator = 0;

    //Arbol generado en analizador sintactico
    public static nodo arbolSintactico;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        GtkScrolledWindow.VScrollbar.ChildVisible = false;
        GtkScrolledWindow1.Vadjustment.ValueChanged += HandleScrollChanged;
        this.textview3.Buffer.AddNotification("cursor-position", this.eventPositionCursor);
        this.GdkWindow.Title = this.currentFile;
        Pango.FontDescription fontdesc = new Pango.FontDescription();
        fontdesc.Size = Convert.ToInt32(14 * Pango.Scale.PangoScale);
        this.textview3.ModifyFont(fontdesc);
        this.textview1.ModifyFont(fontdesc);
        this.textview4.ModifyFont(fontdesc);
        this.textview9.ModifyFont(fontdesc);
        this.textview5.ModifyFont(fontdesc);
        label10.Text = this.currentFile;
        var tag = new TextTag("Special");
        tag.Foreground = "blue"; //Esta propiedad es para que cambie el color
        textview3.Buffer.TagTable.Add(tag);
        var tag2 = new TextTag("Comment");
        tag2.Foreground = "green"; //Esta propiedad es para que cambie el color
        textview3.Buffer.TagTable.Add(tag2);
    }
   

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        if(this.caracteresActuales > this.caracteresEscritos || this.caracteresActuales < this.caracteresEscritos)
        {
            MessageDialog dialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo, "Tiene cambios sin guardar. ¿Desea continuar?");
            if (dialog.Run() == (int)ResponseType.Yes)
            {
                Application.Quit();
            }
            dialog.Destroy();
            a.RetVal = true;
        }
        else
        {
            Application.Quit();
            a.RetVal = true;
        }
       
    }

    void HandleScrollChanged(object sender, EventArgs e)
    {
        Syncronize2Elements();
    }


    protected void OnTextview3KeyReleaseEvent(object o, KeyReleaseEventArgs args)
    {    
            AddLineNumbers();
            Syncronize2Elements();
    }

    public void eventPositionCursor(object o, GLib.NotifyArgs args)
    {
        
        FindMatchesInBuffer(textview3.Buffer);
        IsEditing();
        AddLineNumbers();
        Syncronize2Elements();
    }

    public void FindMatchesInBuffer(TextBuffer buffer)
    {

        textview3.Buffer.RemoveAllTags(buffer.StartIter, buffer.EndIter);

        Gtk.TextIter searchiter = buffer.GetIterAtOffset(0);
        Gtk.TextIter aux = buffer.GetIterAtOffset(0);
        Gtk.TextIter PrimerInicioiter, PrimerFinaliter, SegundoInicioiter, SegundoFinaliter;
        bool inicio = true;

        while (true)
        {
            if (inicio)
            {
                bool word_found = searchiter.ForwardSearch("//", TextSearchFlags.VisibleOnly, out PrimerInicioiter, out PrimerFinaliter, buffer.EndIter);
                if (word_found)
                {
                    inicio = false;
                    textview3.Buffer.ApplyTag("Comment", PrimerInicioiter, PrimerFinaliter);
                    aux = PrimerInicioiter;
                    searchiter = PrimerFinaliter;

                }
                else
                {
                    break;
                }
            }
            else
            {

                bool word_found = searchiter.ForwardSearch("\n", TextSearchFlags.VisibleOnly, out PrimerInicioiter, out PrimerFinaliter, buffer.EndIter);
                if (word_found)
                {
                    textview3.Buffer.ApplyTag("Comment", aux, PrimerFinaliter);
                    inicio = true;
                }
                else
                {
                    textview3.Buffer.ApplyTag("Comment", searchiter, searchiter.Buffer.EndIter);
                    break;
                }
            }
        }

        searchiter = buffer.GetIterAtOffset(0);
        inicio = true;

        while (true)
        {
            if (inicio)
            {
                bool word_found = searchiter.ForwardSearch("/*", TextSearchFlags.VisibleOnly, out PrimerInicioiter, out PrimerFinaliter, buffer.EndIter);
                bool Linea = false;
                
                if (word_found)
                {
                    inicio = false;
                    aux = PrimerInicioiter;
                    searchiter = PrimerFinaliter;

                    if (PrimerInicioiter.BackwardChar())
                    {
                        if (PrimerInicioiter.Char != "\n")
                        {
                            while (PrimerInicioiter.Char != "\n" && !PrimerInicioiter.IsStart)
                            {
                                Console.WriteLine(PrimerInicioiter.CharsInLine);
                                Console.WriteLine(PrimerInicioiter.Char);
                                

                                if (PrimerInicioiter.Char == "/" && Linea == true)
                                {
                                    Console.WriteLine(PrimerInicioiter.Char);
                                    inicio = true;
                                    aux = PrimerFinaliter;
                                    searchiter = PrimerFinaliter;
                                    break;
                                }
                                else
                                {
                                    Linea = false;
                                }
                             
                                if (PrimerInicioiter.Char == "/" && Linea == false)
                                {
                                    Console.WriteLine(PrimerInicioiter.Char);
                                    Linea = true;
                                }

                                PrimerInicioiter.BackwardChar();
                            }
                        }
                    }           

                }
                else
                {
                    break;
                }
            }
            else
            {
                bool word_found = searchiter.ForwardSearch("*/", TextSearchFlags.VisibleOnly, out SegundoInicioiter, out SegundoFinaliter, buffer.EndIter);
                if (word_found)
                {
                    textview3.Buffer.ApplyTag("Comment", aux, SegundoFinaliter);
                    inicio = true;
                    searchiter = SegundoFinaliter;

                }
                else
                {
                    textview3.Buffer.ApplyTag("Comment", aux, buffer.EndIter);
                    break;
                }
            }
        }


        foreach (string word in PalabrasReservadas)
        {

            if (word == String.Empty)
                continue;

            searchiter = buffer.GetIterAtOffset(0);
            while (true)
            {
                Gtk.TextIter startiter, enditer;
                bool word_found = searchiter.ForwardSearch(word, TextSearchFlags.VisibleOnly, out startiter, out enditer, buffer.EndIter);

                if (!word_found)
                {
                    break;
                }
                TextIter start;
                start = startiter;
                if (!start.BackwardChar())
                {
                    Console.WriteLine("entro");
                    char[] arrayChar = enditer.Char.ToCharArray();
                    if (enditer.Char == "" || !Char.IsLetterOrDigit(arrayChar[0])) 
                    {
                        textview3.Buffer.ApplyTag("Special", startiter, enditer);
                        searchiter = enditer;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    char[] arrayCharStart = start.Char.ToCharArray();
                    char[] arrayCharEnd = enditer.Char.ToCharArray();
                    if (enditer.Char == "" || !Char.IsLetterOrDigit(arrayCharEnd[0]))
                    {
                        if (start.Char == "" || !Char.IsLetterOrDigit(arrayCharStart[0]))
                        {
                            textview3.Buffer.ApplyTag("Special", startiter, enditer);
                            searchiter = enditer;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }

    }

    public void positionCursor()
    {
        int line = textview3.Buffer.GetIterAtOffset(textview3.Buffer.CursorPosition).Line;
        int charline = textview3.Buffer.GetIterAtOffset(textview3.Buffer.CursorPosition).LineOffset;
        label11.Text = "[" + (line + 1).ToString() + "," + charline.ToString() + "]";

    }
    protected void OnTextview3KeyPressEvent(object o, KeyPressEventArgs args)
    {      
            AddLineNumbers();
            Syncronize2Elements();     
    }

    public void Syncronize2Elements()
    {
        GtkScrolledWindow.Vadjustment.Value = GtkScrolledWindow1.Vadjustment.Value;
    }

    public void AddLineNumbers()
    {
        
        
        int lines = textview3.Buffer.LineCount;
        positionCursor();
        if (this.lines < lines)
        {
            string lineNumbers = "";
            for (int i = this.lines; i < lines; i++)
            {
                lineNumbers += (i+1) + "\n";
            }
            textview1.Buffer.Insert(textview1.Buffer.EndIter, lineNumbers);
        }else if( this.lines == lines)
        {
            return;
        }
        else
        {
            for (int i = lines; i < this.lines; i++)
            {
                textview1.Buffer.Delete(textview1.Buffer.GetIterAtLine(lines), textview1.Buffer.EndIter);
                
            }
        }
        this.lines = lines;
        

        

    }

    public void IsEditing()
    {
        this.caracteresEscritos = textview3.Buffer.CharCount;

        if (this.caracteresEscritos > this.caracteresActuales || this.caracteresEscritos < this.caracteresActuales)
        {
            this.changed = true;
            this.GdkWindow.Title = this.currentFile + "*";
            label10.Text = this.currentFile + "*";
        }
        else
        {
            this.changed = false;
            this.GdkWindow.Title = this.currentFile;
            label10.Text = this.currentFile;
        }
    }

    protected void OnTextview3ScrollEvent(object o, ScrollEventArgs args)
    {
        Syncronize2Elements();
    }

    protected void OnTextview1ScrollEvent(object o, ScrollEventArgs args)
    {
        Syncronize2Elements();
    }

    protected void OnTextview3PasteClipboard(object sender, EventArgs e)
    {
        AddLineNumbers();
        Syncronize2Elements();
    }

    protected void OnTextview3MoveCursor(object o, MoveCursorArgs args)
    {
        positionCursor();
        Syncronize2Elements();
    }

    protected void OnTextview3CutClipboard(object sender, EventArgs e)
    {
        AddLineNumbers();
    }

    protected void OnTextview3InsertAtCursor(object o, InsertAtCursorArgs args)
    {
        AddLineNumbers();
    }

    protected void OnTextview3MoveFocus(object o, MoveFocusArgs args)
    {
        Syncronize2Elements();
    }

    protected void OnTextview3DeleteFromCursor(object o, DeleteFromCursorArgs args)
    {
        AddLineNumbers();
        Syncronize2Elements();
    }

   
    public void guardarComo()
    {
        FileChooserDialog filechooser = new FileChooserDialog(
            "Save document",
            this,
            FileChooserAction.Save,
            "Cancel", ResponseType.Cancel,
            "Save", ResponseType.Accept);
        if (filechooser.Run() == (int)ResponseType.Accept)
        {
            filechooser.SetFilename("sin titulo.txt");
            using (var savefile = new StreamWriter(filechooser.Filename))
            {
                savefile.WriteLine(textview3.Buffer.Text);
                this.currentFile = filechooser.Filename;
                this.caracteresEscritos = textview3.Buffer.CharCount;
                this.caracteresActuales = textview3.Buffer.CharCount;
                IsEditing();
            }
        }
        filechooser.Destroy();
    }

    protected void OnAbrirActionActivated(object sender, EventArgs e)
    {
        if ( this.caracteresEscritos > this.caracteresActuales || this.caracteresEscritos < this.caracteresActuales)
        {
            dialogNotSaved();
        }
        else
        {
            abrirArchivo();
        }
        
    }

    public void dialogNotSaved()
    {
        MessageDialog dialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo, "Tiene cambios sin guardar. Desea continuar?");
        if (dialog.Run() == (int)ResponseType.Yes)
        {
            abrirArchivo();
        }
        dialog.Destroy();
    }

    public void abrirArchivo()
    {
        
        string r;
        Gtk.FileChooserDialog filechooser =
        new Gtk.FileChooserDialog("Choose the file to open",
            this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept);

        if (filechooser.Run() == (int)ResponseType.Accept)
        {
            this.currentFile = filechooser.Filename;
            StreamReader file = new StreamReader(filechooser.Filename);
            r = file.ReadToEnd();
            file.Close();
            textview3.Buffer.Text = r;

            this.caracteresActuales = textview3.Buffer.CharCount;
            this.caracteresEscritos = textview3.Buffer.CharCount;

            
            AddLineNumbers();
            IsEditing();
        }
        filechooser.Destroy();
        this.changed = false;
        

    }

    protected void OnGuardarComoActionActivated(object sender, EventArgs e)
    {
        guardarComo();
    }

    protected void OnGuardarActionActivated(object sender, EventArgs e)
    {
        if(this.currentFile != "" )
        {
            int charcount = textview3.Buffer.CharCount;
            using (var saveFile = new StreamWriter(this.currentFile))
            {
                saveFile.WriteLine(textview3.Buffer.Text);
            }
            this.changed = false;
            this.caracteresEscritos = charcount;
            this.caracteresActuales = charcount;
            IsEditing();

        }
        else //Cuando se uso una hoja vacia
        {
            guardarComo();
        }
    }

    protected void OnCerrarActionActivated(object sender, EventArgs e)
    {
        if (this.caracteresEscritos < this.caracteresActuales || this.caracteresEscritos > this.caracteresActuales )
        {
            MessageDialog dialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo, "Tiene cambios sin guardar. Desea continuar?");
            if (dialog.Run() == (int)ResponseType.Yes)
            {
                Environment.Exit(0);
            }
            dialog.Destroy();
        }
        else
        {
            Environment.Exit(0);
        }
    }

    protected void OnSaveActionActivated(object sender, EventArgs e)
    {
        OnGuardarActionActivated(null,null);
    }

    protected void OnOpenActionActivated(object sender, EventArgs e)
    {
        OnAbrirActionActivated(null, null);
    }

    protected void OnNewActionActivated(object sender, EventArgs e)
    {
        if(this.caracteresEscritos < this.caracteresActuales || this.caracteresEscritos > this.caracteresActuales)
        {
            MessageDialog dialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo, "Tiene cambios sin guardar. ¿Desea continuar?");
            if (dialog.Run() == (int)ResponseType.Yes)
            {
                this.currentFile = "";
                this.caracteresActuales = 0;
                this.caracteresEscritos = 0;
                textview3.Buffer.Clear();
                AddLineNumbers();
            }
            dialog.Destroy();
        }
        else
        {
           this.currentFile = "";
                this.caracteresActuales = 0;
                this.caracteresEscritos = 0;
                textview3.Buffer.Clear();
                AddLineNumbers(); 
        }
    }

    protected void OnSaveAsActionActivated(object sender, EventArgs e)
    {
        guardarComo();
    }

    protected void OnEjecutarActionActivated(object sender, EventArgs e)
    {
        ejecutarLexico lexicoclase = new ejecutarLexico();
        lexicoclase.ejecutarIde(textview3.Buffer.Text);
        textview9.Buffer.Text = lexicoclase.getListaTokens();
        textview4.Buffer.Text = lexicoclase.getListaErrores();
        listaTokens = lexicoclase.getTokens();
    }

    protected void OnEjecutarAction1Activated(object sender, EventArgs e)
    {
        erroresSintactico = "";
        iterator = 0;
        if(listaTokens.Count <= 0)
        {
            return;
        }

        removeAllColumns(treeview1);

        nodo nodo = new nodo(); //Aqui se ejecuta el analizador SINTACTICO
        nodo = nodo.programa();
        
        textview6.Buffer.Text = erroresSintactico;

        
        Gtk.TreeViewColumn columna = new Gtk.TreeViewColumn();
        columna.Title = "Arbol";


        Gtk.CellRendererText celda = new Gtk.CellRendererText();
        columna.PackStart(celda, true);


        treeview1.AppendColumn(columna);
        columna.AddAttribute(celda, "text", 0);
        Gtk.TreeStore lista = new Gtk.TreeStore(typeof(string));
        verArbol(nodo, lista);
        treeview1.ExpandAll(); //Propiedad para expandir el arbol

        arbolSintactico = nodo; //Asignamos el arbol del analizador sintactico a nuestra variable estatica
    }

    public void verArbol(nodo arbol, TreeStore lista)
    {
        if (arbol != null)
        {
            TreeIter iter1 = lista.AppendValues(arbol.nombre);
            arbol.hijos.ForEach(hijo =>
            {
                verArbol(hijo,lista,iter1);
            });
        }
        treeview1.Model = lista;
    }

    public void verArbol(nodo arbol, TreeStore lista, TreeIter iter)
    {
        if (arbol != null)
        {
            TreeIter iter1 = lista.AppendValues(iter,arbol.nombre);
            arbol.hijos.ForEach(hijo =>
            {
                verArbol(hijo, lista, iter1);
            });
        }
        treeview1.Model = lista;
    }

    public static Token getToken()
    {
        if(iterator < listaTokens.Count)
        {
            return listaTokens[iterator];
        }

        return new Token();        
    }
    public static Token getTokenAnterior()
    {
        if(iterator< listaTokens.Count && iterator!=0)
        {
            return listaTokens[iterator-1];
        }

        return new Token();
    }


    private static void removeAllColumns(TreeView treeView)
    {
        //vaciar el array
        TreeViewColumn[] treeViewColumns = treeView.Columns;
        foreach (TreeViewColumn treeViewColumn in treeViewColumns)
            treeView.RemoveColumn(treeViewColumn);
    }

    protected void OnEjecutarSemantico(object sender, EventArgs e)
    {
        if(arbolSintactico == null)
        {
            return;
        }


        Semantico.fnResetRun(); //Se resetea diccionario y String de errores semanticos
        Semantico semantico = new Semantico(arbolSintactico);
        semantico.mainSintactico();

        buildHashTable();


        //Dibujamos el arbol semantico
        Gtk.TreeViewColumn columna = new Gtk.TreeViewColumn();
        columna.Title = "Arbol";


        Gtk.CellRendererText celda = new Gtk.CellRendererText();
        columna.PackStart(celda, true);


        treeview2.AppendColumn(columna);
        columna.AddAttribute(celda, "text", 0);
        Gtk.TreeStore lista = new Gtk.TreeStore(typeof(string));
        verArbolSemantico(semantico.getArbol(), lista);
        treeview2.ExpandAll(); //Propiedad para expandir el arbol

    }

    public void buildHashTable()
    {

        textview7.Buffer.Text = Semantico.erroresSemanticos;
        string typeString = "", lista = "";
        
        NodeStore store = new Gtk.NodeStore(typeof(MyTreeNode));
        foreach (var item in Semantico.tablaHash)
        {
            switch (item.Value.tipo)
            {
                case TipoNodo.integer:
                    typeString = "Int";
                    break;
                case TipoNodo.float_number:
                    typeString = "Float";
                    break;
                case TipoNodo.boolean:
                    typeString = "Boolean";
                    break;
            }
            lista = "";
            item.Value.lista.ForEach(element =>
            {
                lista += element.ToString() + " ";
            }); 
            store.AddNode(new MyTreeNode(item.Value.nombre, item.Value.location, lista, typeString, item.Value.valor.ToString()));
        }

        nodeview1.NodeStore = store;
        nodeview1.AppendColumn("Nombre", new Gtk.CellRendererText(), "text", 0);
        nodeview1.AppendColumn("Locación", new Gtk.CellRendererText(), "text", 1);
        nodeview1.AppendColumn("Lineas", new Gtk.CellRendererText(), "text", 2);
        nodeview1.AppendColumn("Tipo", new Gtk.CellRendererText(), "text", 3);
        nodeview1.AppendColumn("Valor", new Gtk.CellRendererText(), "text", 4);

    }
    public void verArbolSemantico(nodo arbol, TreeStore lista)
    {
        if (arbol != null)
        {
            TreeIter iter1 = lista.AppendValues(getTypeNode(arbol));
            arbol.hijos.ForEach(hijo =>
            {
                verArbolSemantico(hijo, lista, iter1);
            });
        }
        treeview2.Model = lista;
    }

    public void verArbolSemantico(nodo arbol, TreeStore lista, TreeIter iter)
    {
        if (arbol != null)
        {
            
            TreeIter iter1 = lista.AppendValues(iter, getTypeNode(arbol));
            arbol.hijos.ForEach(hijo =>
            {
                verArbolSemantico(hijo, lista, iter1);
            });
        }
        treeview2.Model = lista;
    }

    public string getTypeNode(nodo arbol)
    {
        string valor = arbol.nombre;
        switch (arbol.tipoNodo)
        {
            case TipoNodo.integer:
                valor += " (Int: " + arbol.valor.ToString() + " )";
                break;
            case TipoNodo.float_number:
                valor += " (Float: " + arbol.valor.ToString() + " )"; 
                break;
            case TipoNodo.boolean:
                valor += " (Bool: " + arbol.valor.ToString() + " )"; 
                break;
        }
        return valor;
    }

}

[Gtk.TreeNode(ListOnly = true)]
public class MyTreeNode : Gtk.TreeNode
{

    public MyTreeNode(string nombre, string location, string lineas, string tipo, string valor)
    {
        this.nombre = nombre;
        this.location = location;
        this.lineas = lineas;
        this.tipo = tipo;
        this.valor = valor;
    }

    [Gtk.TreeNodeValue(Column = 0)]
    public string nombre;

    [Gtk.TreeNodeValue(Column = 1)]
    public string location;

    [Gtk.TreeNodeValue(Column = 2)]
    public string lineas;

    [Gtk.TreeNodeValue(Column = 3)]
    public string tipo;

    [Gtk.TreeNodeValue(Column = 4)]
    public string valor;
}
