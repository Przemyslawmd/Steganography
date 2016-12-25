using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Compression
{
    class HuffmanCodes
    {
         public Dictionary<byte, String> CreateCodesDictionary( NodeCompress root )
        {
            codes = new Dictionary<byte, String>();
            code = new List<Char>();
            GenerateCodes( root, '1' );
            return codes;
        }

        /********************************************************************************/
        /* RECURSIVE METHOD WHICH TRAVERSE TREE AND GENERATES CODES *********************/

        private void GenerateCodes( NodeCompress node, Char token )
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

        /***********************************************************************************/
        /***********************************************************************************/

        private List<Char> code;
        private Dictionary<byte, String> codes;
    }
}
