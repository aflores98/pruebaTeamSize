using Microsoft.AspNetCore.Mvc;
using pruebaTeamSize.Models;


namespace pruebaTeamSize.Controllers
{
    [ApiController]
    [Route("TeamSize")]
    public class TeamSizeController : Controller
    {
        [HttpPost]
        [Route("equializeTeamSize")]
        public List<int> equializeTeamSize(Input input)
        {
            #region[Variables]
            int k = input.k;
            List<int> teamOrdered = input.teamSize;                         //Lista a ordenar
            List<int> teamResult = input.teamSize;                          //Lista resultante
            Dictionary<int, int> dictCount = new Dictionary<int, int>();    //Diccionario para asignar repeticiones del número de miembros en los diferentes equipos
            #endregion

            try
            {
                if (k > teamResult.Count)                           //Valida que el numero a equipos a reducir no exceda el length del array
                    throw new Exception("El número de equipos a reducir (k) excede el número de equipos posibles a reducir");
                else if(k < 0)                                      //Valida que el numero a equipos a reducir no sea negativo
                    throw new Exception("El número de equipos a reducir (k) debe ser entero");
                

                #region[Ordering]
                teamOrdered = teamOrdered.OrderByDescending(x => x).ToList(); //Ordenar los equipos de forma descendente

                for (int i = 0; i < teamOrdered.Count; i++)         //Recorre la lista de equipos para contar repetidos
                {     
                    if (dictCount.ContainsKey(teamOrdered[i]))
                        dictCount[teamOrdered[i]]++;                //Suma 1 repeticiónm al número de miembro de un equipo en el caso ya haya una aparición en el diccionario
                    else
                        dictCount.Add(teamOrdered[i], 1);           //Graba el número de miembros de un equipo con su primera aparición

                }

                Dictionary<int, int> dictCountOrdered = dictCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //Ordenar dictionary de forma descendente

                List<int> keysOrdered = new List<int>();
                keysOrdered.Add(dictCountOrdered.Keys.ToList()[0]);
                keysOrdered.AddRange(dictCountOrdered.Keys.ToList().GetRange(1, dictCountOrdered.Keys.Count - 1).OrderByDescending(x => x)); //Obtener keys de dictionary ordenadas de mayor a menor repetición
                #endregion

                #region[Validations]
                //Variables auxiliares para indices de recorrido
                int auxCount = 0;   
                int auxK = k;
                int temp = 0;
                int temp2 = 0;

                if (k == teamOrdered.Count) //Si se quiere reducir todos los grupos, se reducen siempre a la menor cantidad de miembros
                {
                    keysOrdered = keysOrdered.OrderBy(x => x).ToList();
                }
                else
                {
                    while (teamOrdered.Any() && auxCount < keysOrdered.Count - 1 && keysOrdered[auxCount] >= teamOrdered[0])    //Valida que la mayor cantidad de apariciones de miembros no sea la mayor cantidad de miembros de todos los equipos, caso contrario cambia sus posiciones
                    {
                        temp = keysOrdered[0];

                        if (temp < keysOrdered[auxCount + 1])
                        {
                            temp2 = keysOrdered[auxCount + 1];

                            keysOrdered[auxCount + 1] = keysOrdered[auxCount + 2];
                            keysOrdered[auxCount + 2] = temp2;

                        }
                        else
                        {
                            keysOrdered[0] = keysOrdered[auxCount + 1];
                            keysOrdered[auxCount + 1] = temp;

                            teamOrdered.RemoveAll(x => x == temp);
                            auxCount++;

                            if (auxK > teamResult.Where(x => x == temp).ToList().Count)
                                auxK = auxK - teamResult.Where(x => x == temp).ToList().Count;
                            else
                                break;
                        }
                    }
                }
               
                #endregion

                #region[For - change members]
                for (int i = 1; i < keysOrdered.Count; i++) //Recorre el array para reducir los miembros de los equipos
                {
                    int numRepeat = dictCountOrdered[keysOrdered[i]];

                    if(k > 0 && numRepeat <= k)
                    {                         
                        for (int j = 0; j < teamResult.Count; j++)
                        {
                            if (teamResult[j] == keysOrdered[i])
                            {
                                teamResult[j] = keysOrdered[0];
                            }
                        }

                        k = k - numRepeat;
                    }
                    else if (k > 0 && numRepeat >= k)
                    {
                        for (int j = 0; j < teamResult.Count; j++)
                        {
                            if (k > 0)
                            {
                                if (teamResult[j] == keysOrdered[i])
                                {
                                    teamResult[j] = keysOrdered[0];
                                    k--;
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                    }                      
                }
                #endregion

            }
            catch (Exception e)
            {
                throw new Exception("Ocurrió un error al ejecutar el método equializeTeamSize. \n" + e.Message );
            }
            
            return teamResult;
        }
    }
}
