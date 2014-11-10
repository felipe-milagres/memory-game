using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEditor;

public class ParseXML {
	
	private XmlDocument _xml;
	private TextAsset _textXML;

	public ParseXML( string xmlName ){
		_xml = new XmlDocument();
		_textXML = (TextAsset) Resources.Load( xmlName , typeof(TextAsset) );
		LoadXML( xmlName );
	}

	private void LoadXML( string xmlName ){
		_xml.LoadXml( _textXML.text );
		Debug.Log( "XML LOADED" );
	}

	private void saveChanges(){
		_xml.Save( AssetDatabase.GetAssetPath( _textXML ) );
		Debug.Log( "CHANGES SAVED" );
	}

	public void ReadXML(){
		XmlNode root = _xml.FirstChild;
		foreach( XmlNode node in root.ChildNodes ){
			Debug.Log( "<" + node.LocalName + ">" );
			foreach( XmlNode sub_node in node.ChildNodes ){
				//Debug.Log( "NodoName=" + node.LocalName + " | SubNodoName=" + sub_node.LocalName + " | NodoContent=" + sub_node.InnerText );
				Debug.Log( "   <" + sub_node.LocalName + ">" + sub_node.InnerText + "<" + sub_node.LocalName + ">" );
			}
		}
	}

	public string getElementXML( string elementName ){
		string retorno = null;
		XmlNode root = _xml.FirstChild;
		foreach( XmlNode node in root.ChildNodes ){
			foreach( XmlNode sub_node in node.ChildNodes ){
				if( sub_node.LocalName == elementName )
					retorno = sub_node.InnerText;
			}
		}
		Debug.Log( "ELEMENT: " + retorno );
		return retorno;
	}

	public void changeValueOf( string elementName , string value ){
		XmlNode root = _xml.FirstChild;
		foreach( XmlNode node in root.ChildNodes ){
			foreach( XmlNode sub_node in node.ChildNodes ){
				if( sub_node.LocalName == elementName ){
					sub_node.InnerText = value;
					break;
				}
			}
		}
		saveChanges();
	}

	private void resetXML(){
		XmlNode root = _xml.FirstChild;
		foreach( XmlNode node in root.ChildNodes ){
			foreach( XmlNode sub_node in node.ChildNodes ){
				if( sub_node.LocalName == "best_time" ){ sub_node.InnerText = "99:99"; }
			}
		}
	}

}
