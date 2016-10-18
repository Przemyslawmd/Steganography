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
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        public void SetPol( ref Dictionary<String, String> menu, ref Dictionary<String, String> mes )                            
        {
            menu = menuPl;            
            mes = mesPl;                               
        }

        /*********************************************************************************************************************************/
        
        public void SetEng( ref Dictionary<String, String> menu, ref Dictionary<String, String> mes )
        {
            menu = menuEn;            
            mes = mesEn;                                  
        }
        
        /********************************************************************************************************************************/
        /* MENU *************************************************************************************************************************/

        private void InitializeMenu()
        {
            menuPl = new Dictionary<String, String>();
            menuEn = new Dictionary<String, String>();

            menuPl.Add("file", "Plik");
            menuEn.Add("file", "File");
            menuPl.Add("openGraphic", "Otwórz plik graficzny");
            menuEn.Add("openGraphic", "Open Graphical File");
            menuPl.Add("openFile", "Otwórz plik do ukrycia");
            menuEn.Add("openFile", "Open File to Cover");
            menuPl.Add("saveGraphic", "Zapisz plik graficzny");
            menuEn.Add("saveGraphic", "Save Graphical File");
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

            mesPl.Add( "noText", "Brak tekstu do ukrycia" );
            mesEn.Add( "noText", "There is no text to hide" );
            mesPl.Add( "toManyData", "Za duża liczba bajtów dla wczytanej grafiki" );
            mesEn.Add( "toManyData", "Too many data to be hidden into a loaded graphic" );
            mesPl.Add( "dataCovered", "Dane zostały ukryte w pliku graficznym" );
            mesEn.Add( "dataCovered", "Data was covered in a graphical file successfully" );            
            mesPl.Add( "fileLoaded", "Wczytano plik: " );
            mesEn.Add( "fileLoaded", "A file was loaded: " );
            mesPl.Add( "numUncover", "Liczba odkrytych bajtów: " );
            mesEn.Add( "numUncover", "Number of uncovered bytes: " );
            mesPl.Add( "errorLoadGraphic", "Niepowodzenie wczytania grafiki" );
            mesEn.Add( "errorLoadGraphic", "Loading graphic failed" );
            mesPl.Add( "errorLoadFile", "Niepowodzenie wczytania pliku" );
            mesEn.Add( "errorLoadFile", "Loading file failed" );
            mesPl.Add( "errorSaveGraphic", "Błąd w przypadku zapisywania grafiki pod tą samą nazwą" );
            mesEn.Add( "errorSaveGraphic", "En error when graphis is being saved with the same name" );
            
            // In messages, because there is no worth to maintain one pair of Dictionary for one content
            mesPl.Add( "filterG", "Plik graficzny|*.jpg; *.jpeg; *.gif; *.bmp; *.png" );
            mesEn.Add( "filterG", "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png" );
        }    
               
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        Dictionary<String, String> menuPl;
        Dictionary<String, String> menuEn;        
        Dictionary<String, String> mesPl;
        Dictionary<String, String> mesEn;     
    }
}
