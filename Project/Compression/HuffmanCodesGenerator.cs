
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Steganography.Huffman
{
    class HuffmanCodesGenerator
    {
        public Dictionary< byte, List< Token >> CreateCodesDictionary( Node root )
        {
            GenerateCodes( root, Token.One, true );
            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node, Token token, bool isRoot )
        {
            if ( isRoot == false )
            {
                code.Add( token );
            }

            if ( node.Left != null )
            {
                GenerateCodes( node.Left, Token.Zero, false );
            }
            if ( node.Right != null )
            {
                GenerateCodes( node.Right, Token.One, false );
            }
            else
            {
                codesDictionary.Add( node.ByteValue, new List< Token >( code ) );
            }

            if ( isRoot == false )
            {
                code.RemoveAt( code.Count - 1 );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly List< Token > code = new List< Token >();
        private readonly Dictionary< byte, List< Token >> codesDictionary = new Dictionary< byte, List< Token >>();
    }
}

