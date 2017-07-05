using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Stegan
{
    class HuffmanCodes
    {
        /********************************************************************************/
        /* CREATE DICTIONARY WITH CODES *************************************************/        

        public Dictionary<byte, String> CreateCodesDictionary( NodeCompress root )
        {
            codes = new Dictionary<byte, string>();
            code = new List<char>();
            GenerateCodes( root, '1' );
            return codes;
        }

        public Dictionary<byte, Code> CreateCodesDictionaryNew( NodeCompress root )
        {
            codesNew = new Dictionary<byte, Code>();
            codeNew = new Code();
            GenerateCodesNew( root, true );
            return codesNew;
        }

        /********************************************************************************/
        /* GENERATE CODES ***************************************************************/
        // Recursive method that traverses tree and generates codes 

        private void GenerateCodes( Node node, char token )
        {
            code.Add( token );

            if ( node.Left != null )
                GenerateCodes( node.Left, '0' );
            if ( node.Right != null )
                GenerateCodes( node.Right, '1' );
            else
                codes.Add( node.ByteValue, String.Concat( code ) );

            code.RemoveAt( code.Count - 1 );
        }


        private void GenerateCodesNew( Node node, bool token )
        {
            codeNew.Add( token );

            if ( node.Left != null )
                GenerateCodesNew( node.Left, false );
            if ( node.Right != null )
                GenerateCodesNew( node.Right, true );
            else
                codesNew.Add( node.ByteValue, new Code( codeNew ));

            codeNew.Remove();
        }

        /***********************************************************************************/
        /***********************************************************************************/

        // In some extreme cases, code may be very long, that it is used List<Char> instead of, 
        // for example, Long type 
        private List<char> code;
        Code codeNew;

        private Dictionary<byte, Code> codesNew;
        private Dictionary<byte, string> codes;
    }
}
