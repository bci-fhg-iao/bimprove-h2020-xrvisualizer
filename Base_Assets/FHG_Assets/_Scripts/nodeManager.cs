using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(materialManager))]

public class nodeManager : MonoBehaviour
{

    public Transform m_rootObject;
    public bool m_Original_Material = true;

    UnityEngine.UI.Text BIMdataTXT = null;

    private Ray m_ray;
    private RaycastHit m_hit;

    public GameObject m_selection_object;
    GameObject m_GUI_right;

    materialManager m_materialManager;

    Stack<GameObject> m_hiddenObjects;
    string[] m_marked_objects; //betroffene Bauteile aus BCF-Bericht

    bool m_initOK = false;

    Dictionary<string, GameObject> m_components = null;

    Camera m_event_camera = null;
    Camera m_camera_right = null;
    public bool m_mono = true;
		public bool m_vive = true;

	
    //int m_cam_count = 0;
    //Camera[] m_active_cams;

    Vector3 m_screen_pos;

    void Start()
    {		

				 m_screen_pos = new Vector3();

        m_components = new Dictionary<string, GameObject>();
        m_components.Add("Architektur", null);
        m_components.Add("Konstruktion", null);
        m_components.Add("Elektro", null);
        m_components.Add("HLSK", null);
        m_components.Add("Moebel", null);
        m_components.Add("Stahlbau", null);
        m_components.Add("Kueche", null);

        //alle Geometrie-Knoten erhalten Collider und Material-Objekt
        if (m_rootObject != null)
        {
            foreach (Transform t in m_rootObject.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                Renderer childRenderer = t.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    t.gameObject.AddComponent<MeshCollider>();
                    t.gameObject.AddComponent<multiMaterialNode>();
                }
            }
        }
        else
        {
            Debug.Log("ERROR: mousePointer has no root Object for Collider-Assert");
        }

        m_hiddenObjects = new Stack<GameObject>();
    }

    public bool initOK()
    {
        return m_initOK;
    }

    bool init()
    {
        bool init_ok = true;

        m_materialManager = GetComponent<materialManager>();
        GameObject found_obj;

        if (BIMdataTXT == null)
        {
            found_obj = GameObject.Find("BIM-text");
            if (found_obj != null)
            {
                BIMdataTXT = found_obj.GetComponent<UnityEngine.UI.Text>();
            }
            else
            {
                Debug.Log("[nodeManager]->init ERROR: object not found BIMdataTXT ");
                init_ok = false;
            }
        }

        if (m_GUI_right == null)
        {
            found_obj = GameObject.Find("GUI-Right");
            if (init_ok && found_obj != null)
            {
                m_GUI_right = found_obj;
                m_GUI_right.SetActive(false);
            }
            else
            {
                Debug.Log("[nodeManager]->init ERROR: object not found GUI-Right");
                init_ok = false;
            }
        }

        if (m_event_camera == null)
        {
            if (m_mono)
                found_obj = GameObject.Find("Main Camera");
            else if(m_vive)
						{
							found_obj = GameObject.Find("Camera (eye)");
						}
						else
						{
							found_obj = GameObject.Find("left");
						}

            if (init_ok && found_obj != null)
            {
                m_event_camera = found_obj.GetComponent<Camera>();
                //add character controller
                found_obj.AddComponent<CharacterController>();

                Debug.Log("Event-Camera is: " + m_event_camera.name);

                if (m_mono == false)
                {
                    found_obj = GameObject.Find("right");
                    if (found_obj != null)
                    {
                        m_camera_right = found_obj.GetComponent<Camera>();
                    }
                    else
                        init_ok = false;
                }
                else
                {
                    //  init_ok = true;
                }
            }
        }

        if (initNodes() == false)
            init_ok = false;

        if (init_ok == true && m_materialManager != null && BIMdataTXT != null && m_event_camera != null) //&& m_camera_right != null)  && m_components != null                                                                                                                 // if (m_materialManager != null && BIMdataTXT != null && m_components != null)
        {
            //TODO: Farbe nach Kategorie 


            //List<string> compo_keys = new List<string>(m_components.Keys);
            //if (m_Original_Material == false)
            //{
            //    //Materialen für alle 3D-Modell-Komponenten initialisieren                   
            //    foreach (string key in compo_keys)
            //    {
            //        //Debug.Log("[printChilds key]: " + key);
            //        initMaterials(key);
            //    }
            //}
            //else
            //{
            //    //Materialen für alle 3D-Modell-Komponenten initialisieren                   
            //    foreach (string key in compo_keys)
            //    {
            //        //Debug.Log("[printChilds key]: " + key);
            //        initOriginalMaterial(key);
            //    }
            //}

            ////Material-Modus für alle 3D-Modell-Komponenten auf "simple setzen"
            //foreach (string key in compo_keys)
            //{
            //    //Debug.Log("[printChilds key]: " + key);
            //    textureMode(key, false);
            //}
            // m_event_camera.transform.localPosition

            return true;
        }
        else
            return false;
    }

    void Update()
    {
        if (!m_initOK)
        {
            m_initOK = init();
        }
        else
        {


					if (m_mono)
					{
						select();
				}
				//selectVive( ... ) wird von ViveInput bzw. VRNavigation aufegrufen
				//else if(m_vive)
				//{
				//	selectVIVE(...) 
				//}           
        }
    }


		private void select()
		{
		// Auswählen eines Objekts, wenn kein Klick auf GUI, Klick ins Leere hebt Auswahl auf:
			if (m_initOK && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
			{
				// unselected last obj
				highlightObj(false);

				bool isHit = false;

				if (!m_mono && Input.mousePosition.x >= Screen.width / 2.0f)
				{
					m_screen_pos.x = Input.mousePosition.x - (Screen.width / 2.0f);
					m_screen_pos.y = Input.mousePosition.y;
					Vector3 pos_left = new Vector3(m_screen_pos.x, m_screen_pos.y, 1);
					isHit = Physics.Raycast(m_event_camera.ScreenPointToRay(pos_left), out m_hit);
				}
				else
				{
					m_screen_pos.x = Input.mousePosition.x;
					m_screen_pos.y = Input.mousePosition.y;
					isHit = Physics.Raycast(m_event_camera.ScreenPointToRay(Input.mousePosition), out m_hit);
				}


				if (isHit)
				{
					if (m_hit.collider != null)
					{
						m_selection_object = m_hit.collider.gameObject;

						highlightObj(true);
						metadata myData = m_hit.collider.gameObject.GetComponent<metadata>();

						if (BIMdataTXT != null && m_GUI_right != null)
						{
							m_GUI_right.SetActive(true); //Objekt ausgewählt -> BIM-Daten einblenden
							if (myData != null)
							{
								BIMdataTXT.text = formatData(myData);
							}
							else
							{
								BIMdataTXT.text = getObjectLabel(m_selection_object);//"Keine BIM-Daten -> Objektname anzeigen";

								if (m_selection_object.transform.parent.gameObject != null && (BIMdataTXT.text.Contains("mesh") || BIMdataTXT.text.Contains("lines") || BIMdataTXT.text.StartsWith("a_")))
								{
									BIMdataTXT.text = getObjectLabel(m_selection_object.transform.parent.gameObject);
								}

							}
						}
					}
				}
				else
				{
					if (BIMdataTXT != null && m_GUI_right != null)
					{
						m_GUI_right.SetActive(false); //kein Objekt ausgewählt -> BIM-Daten ausblenden
					}
				}
			}

			// Verstecken eines ausgewählten Objekts bei Rechtsklick, wenn kein Klick auf GUI
			if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && !((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
			{
				if (m_selection_object != null)
				{
						// unselected last obj
						highlightObj(false);
						m_hiddenObjects.Push(m_selection_object);
						m_selection_object.SetActive(false);
						m_selection_object = null;
				}

						if (BIMdataTXT != null && m_GUI_right != null)
						{
								m_GUI_right.SetActive(false); //kein Objekt ausgewählt -> BIM-Daten ausblenden
						}
				}

				// Anzeigen aller versteckten Objekte bei Mittelklick, wenn kein Klick auf GUI
				//if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())

				// Anzeigen aller versteckten Objekte bei Doppelklick rechts, wenn kein Klick auf GUI
				if (Input.GetKeyDown(KeyCode.U) || (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject()) && !((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
				{
						unhideObjects();
				}
		}

    public void selectVIVE(bool select, bool hide, bool unhide, RaycastHit hitobject, bool ishit)
    {
		//Debug.Log("selectVIVE");

		if(Input.GetKeyDown(KeyCode.U))
		{
			Debug.Log("Input.GetKeyDown(KeyCode.U)");
			highlightObj(false);
		}
		// Auswählen eines Objekts, wenn kein Klick auf oberen Touchpad bereich, Klick ins Leere hebt Auswahl auf:
		if (ishit)
		{
			if (select == true)
			{
				// unselected last obj
				Debug.Log("Select new object");
				highlightObj(false);

				if (hitobject.collider != null)
				{
					m_selection_object = hitobject.collider.gameObject;

					highlightObj(true);
					metadata myData = hitobject.collider.gameObject.GetComponent<metadata>();

					if (BIMdataTXT != null && m_GUI_right != null)
					{
						m_GUI_right.SetActive(true); //Objekt ausgewählt -> BIM-Daten einblenden
						if (myData != null)
						{
							BIMdataTXT.text = formatData(myData);
                            Debug.Log(BIMdataTXT.text);
						}
						else
						{
							BIMdataTXT.text = getObjectLabel(m_selection_object);//"Keine BIM-Daten -> Objektname anzeigen";

							if (m_selection_object.transform.parent.gameObject != null && (BIMdataTXT.text.Contains("mesh") || BIMdataTXT.text.Contains("lines") || BIMdataTXT.text.StartsWith("a_")))
							{
								BIMdataTXT.text = getObjectLabel(m_selection_object.transform.parent.gameObject);
							}
						}
					}
				}

				else
				{
					if (BIMdataTXT != null && m_GUI_right != null)
					{
						m_GUI_right.SetActive(false); //kein Objekt ausgewählt -> BIM-Daten ausblenden
					}
				}
			}
		}
					
				

				// Verstecken eines ausgewählten Objekts bei untererteil des Tpuchpads, wenn kein Klick auf GUI
			
				if(hide == true)
				{
						highlightObj(false);
						if (m_selection_object != null)
						{						
							// unselected last obj
							highlightObj(false);
							m_hiddenObjects.Push(m_selection_object);
							m_selection_object.SetActive(false);
							m_selection_object = null;
						}

						if (BIMdataTXT != null && m_GUI_right != null)
						{
							m_GUI_right.SetActive(false); //kein Objekt ausgewählt -> BIM-Daten ausblenden
						}
				}	
				
				// Anzeigen aller versteckten Objekte bei trigger, wenn kein Klick auf GUI		
				if(unhide == true)
				{
		
						highlightObj(false);
						unhideObjects();
					if (BIMdataTXT != null && m_GUI_right != null)
					{
						m_GUI_right.SetActive(false); //kein Objekt ausgewählt -> BIM-Daten ausblenden
					}
				}
		}

		void unhideObjects()
		{
				GameObject myObject;
				while (m_hiddenObjects.Count > 0)
				{
						myObject = m_hiddenObjects.Pop();
						myObject.SetActive(true);
				}
		}


    string getObjectLabel(GameObject obj)
    {
        string label = "Bauteilname: -";

        if (obj.name.Length > 0)
        {
            string obj_name = obj.name;
            string ID = "";
            //FBX: ID in [123456)
            if (obj_name.Contains("[") && obj_name.Contains("["))
            {
                ID = obj_name.Between("[", "]");
                obj_name = obj_name.Before("[");
            }
            //DWF: ID in _123456]
            else
            {
                ID = obj_name.Between("_", "]");
                int last = obj_name.LastIndexOf("_");

                if (last > 0)
                    obj_name = obj_name.Substring(0, obj_name.LastIndexOf("_"));
            }

            label = obj_name + "\n\nID: " + ID;
           
        }

        return label;
    }


    public void textureMode(string component, bool showTextures)
    {
        if (m_components[component] != null)
        {
            foreach (Transform t in m_components[component].GetComponentsInChildren<Transform>(true)) //include inactive
            {
                multiMaterialNode node = t.GetComponent<multiMaterialNode>();
                if (node != null)
                {
                    node.setTextureMode(showTextures);
                }
            }
        }
        else
            Debug.Log("ERROR: [nodeManager->textureMode] Komponente " + component + " nicht vorhanden");
    }


    public void setComponents(Dictionary<string, GameObject> compos)
    {
        m_components = compos;
    }

    //void initMaterials(string component)
    //{
    //    foreach (Transform t in m_components[component].GetComponentsInChildren<Transform>(true)) //include inactive
    //    {
    //        multiMaterialNode node = t.GetComponent<multiMaterialNode>();
    //        if (node != null)
    //        {
    //            //Debug.Log(t.gameObject.name);
    //            node.setMaterial(materialVariant.simple, m_materialManager.getMaterial(t.gameObject.name, materialVariant.simple, component));
    //            node.setMaterial(materialVariant.textured, m_materialManager.getMaterial(t.gameObject.name, materialVariant.textured));
    //        }
    //    }
    //}

    bool initNodes()
    {
        if (m_rootObject != null)
        {
            foreach (Transform t in m_rootObject.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                multiMaterialNode node = t.GetComponent<multiMaterialNode>();
                if (node != null)
                {
                    //Debug.Log(t.gameObject.name);
                    node.setOriginalMaterial();
                }
            }
            return true;
        }
        else
        {
            Debug.Log("ERROR [nodeManager->initNodes] nor root object");
            return false;
        }
    }


    void initOriginalMaterial(string component)
    {
        foreach (Transform t in m_components[component].GetComponentsInChildren<Transform>(true)) //include inactive
        {
            multiMaterialNode node = t.GetComponent<multiMaterialNode>();
            if (node != null)
            {
                //Debug.Log(t.gameObject.name);
                node.setOriginalMaterial();
            }
        }
    }

    public void setWireframe(bool isWireframe, string component)
    {
        Debug.Log("setWireframe: " + isWireframe);
        if (m_components[component] != null)
        {
            foreach (Transform t in m_components[component].GetComponentsInChildren<Transform>(true)) //include inactive
            {
                multiMaterialNode childWire = t.GetComponent<multiMaterialNode>();
                if (childWire != null)
                {
                    childWire.setWireframeMode(isWireframe);
                }
            }
        }
        else
            Debug.Log("ERROR: [nodeManager->setWireframe_Arch] Komponente " + component + " nicht vorhanden");
    }

    public void displayObjectAsWireframe(bool isWireframe, GameObject obj)
    {
        Debug.Log("setWireframe: " + isWireframe);
        if (obj != null)
        {
            foreach (Transform t in obj.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                multiMaterialNode childWire = t.GetComponent<multiMaterialNode>();
                if (childWire != null)
                {
                    childWire.setWireframeMode(isWireframe);
                }
            }
        }
        else
            Debug.Log("ERROR: [nodeManager->displayObjectAsWireframe] no GameObject");
    }

    public void setMarkedObjects(string[] markedObj)
    {
        if (markedObj != null)
        {
            setMarking(false);
            m_marked_objects = markedObj;
            setMarking(true);
        }
    }

    void setMarking(bool isMarked)
    {
        if (m_marked_objects != null)
        {
            for (int i = 0; i < m_marked_objects.Length; i++)
            {
                multiMaterialNode myNode = null;
                foreach (Transform t in m_rootObject.GetComponentsInChildren<Transform>(true)) //include inactive
                {
                    if (t.gameObject.name.Contains(m_marked_objects[i]))
                    {
                        //Debug.Log("Object found: " + t.gameObject.name + "  Marked: " + isMarked);
                        myNode = t.gameObject.GetComponent<multiMaterialNode>();
                        break;
                    }
                }

                if (myNode != null)
                {
                    myNode.setMarked(isMarked);

                }
            }
        }
    }


    void highlightObj(bool isSelected)
    {		

				if (m_selection_object != null)
        {				
						multiMaterialNode mymultiMaterialNode = m_selection_object.GetComponent<multiMaterialNode>();
            if (mymultiMaterialNode != null)
            {
								mymultiMaterialNode.setSelected(isSelected);
            }
        }

    }

    string formatData(metadata inputData)
    {
        string output = "Keine Daten";

        if (inputData.values.Length == inputData.keys.Length)
        {
            Debug.Log("Length OK");
            output = "";
            int lengthMin = inputData.keys[0].Length;
            int lengthMax = inputData.keys[0].Length;

            int length = 0;

            for (int i = 0; i < inputData.keys.Length; i++)
            {
                length = inputData.keys[i].Length;

                if (length < lengthMin)
                {
                    lengthMin = length;
                }
                else
                {
                    if (length > lengthMax)
                    {
                        lengthMax = length;
                    }
                }
            }
            Debug.Log("Min: " + lengthMin + "   Max: " + lengthMax);

            int delta = 0;
            for (int i = 0; i < inputData.keys.Length; i++)
            {
                delta = lengthMax - inputData.keys[i].Length;
                //output = output + inputData.keys[i] + getTabs(delta, 3) + inputData.values[i] + "\n";
                output = output + getUnified(lengthMax, inputData.keys[i]) + "\t" + inputData.values[i] + "\n";
            }

        }
        else
        {
            Debug.Log(" Error: formatWithTabs Headers: " + inputData.keys.Length + "   Values: " + inputData.values.Length);
        }

        return output;
    }

    string getUnified(int maxLength, string inputTXT)
    {
        if (inputTXT.Length > maxLength)
        {
            Debug.Log("Error getUnified: Length of iput \"" + inputTXT + "\" is greater than input size: " + maxLength);
        }
        else
        {
            int delta = maxLength - inputTXT.Length;

            for (int i = 0; i < delta; i++)
            {
                inputTXT = inputTXT + " ";
            }
        }


        return inputTXT;
    }

    string getBIMdata(metadata BIMdata)
    {
        string BIMtxt = "no BIM-Data";
        if (BIMdata != null && BIMdata.keys.Length == BIMdata.values.Length)
        {
            BIMtxt = "";
            for (int i = 0; i < BIMdata.keys.Length; i++)
            {
                if (BIMdata.values[i] != "")
                {
                    BIMtxt = BIMtxt + BIMdata.keys[i] + "\t" + "\t" + "\t" + BIMdata.values[i] + "\n";
                }
            }
        }
        return BIMtxt;
    }
}
