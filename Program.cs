using static System.Console;

//MATRICES 
int tamañoTabla = 5;
int[,] original = new int[tamañoTabla,tamañoTabla];

int[,] tableroRobot = new int[tamañoTabla, tamañoTabla];

//VALORES
int contadorPasos = 0; //Contamos cuántos pasos da el robot

int mineralesRecolectados = 0;


//RANDOM
Random rnd = new Random();

// ==== INICIO ====
AsignarMinerales(original); //Ya tenemos minerales en la tabla
int contadorMinerales = ContarMinerales(original); //Contamos cuantos materiales hay en el mapa


// ===POSICIÓN INICIAL ROBOT (empezamos en el centro) ===
int robotX = tamañoTabla / 2;
int robotY = tamañoTabla / 2;

// === DIRECCIÓN INICIAL ROBOT (empezamos hacia arriba) ===
int dirX = 0;
int dirY = 1;

CambiarDirecciónAleatoria(ref dirX, ref dirY);

//Empezamos la simulación
EmpezarSimulación(); 

return;

//=== LÓGICA PRINCIPAL ===


void EmpezarSimulación()
{
    while (contadorPasos <= 40 && contadorMinerales > 0)
    {
        Clear();
        WriteLine($"==== PASO {contadorPasos + 1} ===="); //+1 para que sea "bonito" de ver
        
        bool intentarRecolectar = rnd.NextDouble() < 0.5; //50 % Probabilidad
        int recolectadoAhora = 0;

        if (intentarRecolectar) //Si recolectamos, hacemos to esto
        {
            if (original[robotX, robotY] > 0) //Si hay minerales, entro
            {
                if (original[robotX, robotY] >= 2) //Si hay más de 2, restamos 2 etc...
                {
                    recolectadoAhora = 2;
                }
                else //Si hay menos de 2, hacemos esto
                {
                    recolectadoAhora = original[robotX, robotY];
                }
                original[robotX, robotY] -= recolectadoAhora;
                contadorMinerales -= recolectadoAhora;
                
                mineralesRecolectados += recolectadoAhora;
            }
        }
        else //Si no hay minerales, cambiamos
        {
            CambiarDirecciónAleatoria(ref dirX, ref dirY);
        }

        WriteLine($"POSICIÓN: Fila {robotY}, Columna {robotX}");

        if (intentarRecolectar) //Mostrar si hemos recogido minerales
        {
            WriteLine($"Está recolectando. Se han recolectado {recolectadoAhora} minerales");
        }
        else
        {
            WriteLine("No se ha recolectado nada");
            
        }
        WriteLine();


        MostrarTableroRobot(robotX, robotY);

        while (EsFueraDeTabla(robotX + dirX, robotY + dirY))
        {
            CambiarDirecciónAleatoria(ref dirX, ref dirY);
        }
        
        robotX += dirX;
        robotY += dirY; //Movemos al robot

        contadorPasos++;

        WriteLine("Presiona cualquier tecla para continuar");
        ReadLine();
    }
    MostrarResumenFinal();
}

bool EsFueraDeTabla(int x, int y)
{
    // Devuelve VERDADERO si la siguiente coordenada se sale del mapa por cualquier lado
    return x >= tamañoTabla || y >= tamañoTabla || x < 0 || y < 0;
}

int ContarMinerales(int[,] valores)
{
    int suma = 0;
    for (int i = 0; i < valores.GetLength(0); i++)
    {
        for (int j = 0; j < valores.GetLength(1); j++)
        {
            suma += valores[i, j];
        }
    }
    return suma;
}


void AsignarMinerales(int[,] valores)
{
    for (int i = 0; i < valores.GetLength(0); i++)
    {
        for (int j = 0; j < valores.GetLength(1); j++)
        {
            valores[i, j] = rnd.Next(0, 21);
        }
    }
}

void CambiarDirecciónAleatoria(ref int dx, ref int dy)
{
    do
    {
        dx = rnd.Next(-1, 2);
        dy = rnd.Next(-1, 2);
        
    } while (dx == 0 && dy == 0); // Me aseguro de que no se quede quieto (0, 0)
}

void MostrarTableroRobot(int robX, int robY)
{
    for (int i = 0; i < tamañoTabla; i++)
    {
        for (int j = 0; j < tamañoTabla; j++)
        {
            if (i == robY && j == robX)
            {
                Write("[R]"); //La R es dónde está el robot (podría usar emojis, como el profe)
            }
            else
            {
                Write("[ ]");
            }

            
        }
        WriteLine();
    }
}


void MostrarResumenFinal()
{
    Clear();
    WriteLine("    RESUMEN DE EXPLORACIÓN    ");
    WriteLine("==============================");
    WriteLine($"Tiempo transcurrido (Pasos): {contadorPasos}");
    WriteLine($"Minerales recolectados en total: {mineralesRecolectados}");
    WriteLine($"Posición final del robot: Fila {robotY}, Columna {robotX}");
    WriteLine($"Motivo del final: {(contadorMinerales == 0 ? "Minerales agotados" : "Tiempo máximo alcanzado")}");
    WriteLine();
    WriteLine("ASÍ QUEDA LA TABLA FINAL");
    
    for (int i = 0; i < original.GetLength(0); i++)
    {
        for (int j = 0; j < original.GetLength(1); j++)
        {
            Write($"[{original[i, j]}]"); 
        }
        WriteLine();
    }
}