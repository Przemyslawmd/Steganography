using System;
using System.Collections.Generic;
using System.Text;

namespace Stegan
{
    class Labels
    {           
        public Labels()
        {
            InitializeMenu();
            InitializeMessages();
            InitializeAbout();
            InitializeSettings();            
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        public void SetPol( ref Dictionary<String, String> menu, ref Dictionary<String, String> mes, 
                            ref Dictionary<String, String> about, ref Dictionary<String, String> set)
        {
            menu = menuPl;            
            mes = mesPl;
            about = aboutPl;
            set = setPl;            
        }

        /*********************************************************************************************************************************/
        
        public void SetEng( ref Dictionary<String, String> menu, ref Dictionary<String, String> mes,
                            ref Dictionary<String, String> about, ref Dictionary<String, String> set)
        {
            menu = menuEn;            
            mes = mesEn;
            about = aboutEn;
            set = setEn;            
        }
        
        /********************************************************************************************************************************/
        /* MENU *************************************************************************************************************************/

        private void InitializeMenu()
        {
            menuPl = new Dictionary<String, String>();
            menuEn = new Dictionary<String, String>();

            menuPl.Add("file", "Plik");
            menuEn.Add("file", "File");
            menuPl.Add("openG", "Otwórz plik graficzny");
            menuEn.Add("openG", "Open Graphical File");
            menuPl.Add("openFile", "Otwórz plik do ukrycia");
            menuEn.Add("openFile", "Open File to Cover");
            menuPl.Add("saveG", "Zapisz plik graficzny");
            menuEn.Add("saveG", "Save Graphical File");
            menuPl.Add("saveFile", "Zapisz odkryte dane w pliku");
            menuEn.Add("saveFile", "Save Uncovered Data as File");
            menuPl.Add("remG", "Usuń plik graficzny");
            menuEn.Add("remG", "Remove Graphical File");
            menuPl.Add("remF", "Usuń dane odkryte/do ukrycia");
            menuEn.Add("remF", "Remove Uncovered/to Cover Data");
            menuPl.Add("remT", "Usuń tekst w kontrolce");
            menuEn.Add("remT", "Remove Text in Control");

            menuPl.Add("act", "Akcja");
            menuEn.Add("act", "Action");
            menuPl.Add("coverT", "Ukryj tekst");
            menuEn.Add("coverT", "Cover Text");
            menuPl.Add("coverF", "Ukryj dane z pliku");
            menuEn.Add("coverF", "Cover Data from File");
            menuPl.Add("uncoverT", "Odkryj tekst");
            menuEn.Add("uncoverT", "Uncover Text");
            menuPl.Add("uncoverF", "Odkryj dane");
            menuEn.Add("uncoverF", "Uncover Data");

            menuPl.Add("set", "Ustawienia");
            menuEn.Add("set", "Settings");
            menuPl.Add("info", "O aplikacji");
            menuEn.Add("info", "About");              
        }              
        
        /******************************************************************************************************************************/
        /* MESSAGES *******************************************************************************************************************/

        private void InitializeMessages()
        {
            mesPl = new Dictionary<String, String>(8);
            mesEn = new Dictionary<String, String>(8);

            mesPl.Add("noText", "Brak tekstu do ukrycia.");
            mesEn.Add("noText", "There is no text to hide.");
            mesPl.Add("toManyData", "Za duża liczba bajtów dla wczytanej grafiki.");
            mesEn.Add("toManyData", "Too many data to be hidden into a loaded graphic.");
            mesPl.Add("dataCovered", "Dane zostały ukryte w pliku graficznym.");
            mesEn.Add("dataCovered", "Data was covered in a graphical file successfully.");
            mesPl.Add("mes6", "Niepoprawne dane wejściowe.");
            mesEn.Add("mes6", "Improper input data.");
            mesPl.Add("fileLoaded", "Wczytano plik: ");
            mesEn.Add("fileLoaded", "A file was loaded: ");
            mesPl.Add("numUncover", "Liczba odkrytych bajtów: ");
            mesEn.Add("numUncover", "Number of uncovered bytes: ");
            mesPl.Add("failureG", "Niepowodzenie wczytania grafiki");
            mesEn.Add("failureG", "Loading graphic failed");
            mesPl.Add("failureF", "Niepowodzenie wczytania pliku");
            mesEn.Add("failureF", "Loading file failed");

            // In messages, because there is no worth to maintain one pair of Dictionary for one content
            mesPl.Add("filterG", "Plik graficzny|*.jpg; *.jpeg; *.gif; *.bmp; *.png");
            mesEn.Add("filterG", "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png");
        }        
        
        /******************************************************************************************************************************/
        /* ABOUT **********************************************************************************************************************/

        private void InitializeAbout()
        {
            aboutPl = new Dictionary<String, String>(4);
            aboutEn = new Dictionary<String, String>(4);            
            
            aboutPl.Add("description", "<pre>Aplikacja do steganografii</pre>");
            aboutEn.Add("description", "<pre>Steganography application</pre>");
            aboutPl.Add("author", "<pre>Autor:          Przemysław Madej, Kraków 2016</pre>");
            aboutEn.Add("author", "<pre>Author:         Przemysław Madej, Cracow 2016</pre>");
            aboutPl.Add("web", "<pre>Strona domowa:  http://przemeknet.pl</pre>");
            aboutEn.Add("web", "<pre>Home page:      http://przemeknet.pl</pre>");            
        }

        /******************************************************************************************************************************/
        /* SETTINGS********************************************************************************************************************/

        private void InitializeSettings()
        {
            setPl = new Dictionary<String, String>(3);
            setEn = new Dictionary<String, String>(3);
            
            setPl.Add("lan", "Język");
            setEn.Add("lan", "Language");
            setPl.Add("accept", "Akceptuj");
            setEn.Add("accept", "Accept");
            setPl.Add("compress", "Kompresja danych");
            setEn.Add("compress", "Data compression");
        }

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        Dictionary<String, String> menuPl;
        Dictionary<String, String> menuEn;
        Dictionary<String, String> aboutPl;
        Dictionary<String, String> aboutEn;
        Dictionary<String, String> mesPl;
        Dictionary<String, String> mesEn;        
        Dictionary<String, String> setPl;
        Dictionary<String, String> setEn;
    }
}
