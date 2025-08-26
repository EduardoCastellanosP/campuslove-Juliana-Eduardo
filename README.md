# 💘 CampusLove (Consola .NET)

App de consola tipo “dating” para practicar C#, Entity Framework Core y arquitectura por capas. Permite:
- Crear cuentas (login fijo: **usuario + clave**).
- Registrar/Actualizar **perfil** (nombre real, email, edad, género, profesión, intereses, frase).
- Dar **like/dislike** (una sola reacción por persona).
- Ver **perfiles** y **matches** (likes recíprocos).
- Ver **estadísticas** (quién tiene más likes, dislikes y matches).

> Autores: **Juliana Andrea Pallares Novoa** y **Eduardo Elias Castellanos Picon**

---

## 🧠 TL;DR

- **Usuario** → solo login (nombre de usuario y clave). **No se actualiza** desde el menú de perfil.
- **Dato** → ficha personal (nombre real, email, edad, etc.). **Aquí sí se actualiza**.
- **Like** → reacciones: 1 por par (no puedes like + dislike al mismo usuario).
- **Matches** → existe cuando A da like a B **y** B da like a A.
- **likes_disponibles** → se descuenta en cada reacción.

---

## 🧱 Stack

- .NET (C#) Console App
- Entity Framework Core (Pomelo MySQL provider)
- Arquitectura por módulos (Domain, Application, Infrastructure, UI)

> *Este README evita fragmentos de base de datos a petición: no incluye SQL.*

---

## 🗂️ Estructura del proyecto

<img width="689" height="593" alt="Captura de pantalla 2025-08-26 001452" src="https://github.com/user-attachments/assets/da5eca0a-f021-4426-9e36-44f5618bc312" />



---

## 🧭 Flujo de uso

### Menú Usuario
- **1) Registrar usuario**: crea la cuenta (nombre de usuario + clave).
- **2) Iniciar sesión**: si es correcto, pasa al Menú Datos.

### Menú Datos
- **1) Registrar datos (perfil)**: crea tu ficha personal (NO toca login).
- **2) Actualizar datos (perfil)**: modifica ficha personal (NO toca login).
- **3) Ver Perfiles**: navega con `N/P`, reacciona con `L` (like) o `D` (dislike). Resta **1** de `likes_disponibles`.
- **4) Ver matches**: lista likes recíprocos y sugiere iniciar conversación.
- **5) Ver estadísticas 📊**: muestra nombre del usuario con más likes, con más dislikes y con más matches.
- **6) Volver**.

---

## 🔑 Reglas de negocio clave

- **Login inmutable**: el nombre de usuario y la clave **no se cambian** desde “Actualizar datos”.
- **Perfil editable**: nombre real, email, edad, género, profesión, intereses y frase se actualizan en `Dato`.
- **Una reacción por usuario**: si ya reaccionaste a X, no puedes volver a reaccionar a X (ni cambiar el tipo).
- **likes_disponibles**: decrementa a 0 y luego impide nuevas reacciones.

---

## 🛠️ Notas de implementación

- **Separación de servicios**:
  - `UsuarioService`: solo **crear** la cuenta (login). No implementes “actualizar login”.
  - `DatosService`: registrar/actualizar **perfil** (solo `Dato`).
- **Tracking en EF Core**:
  - Para **mostrar**: usa `AsNoTracking()`.
  - Para **actualizar**: carga una sola instancia trackeada **o** desadjunta la duplicada antes de `Attach/Update`.
  - El `DatoRepository` incluye lógica para evitar:  
    *“The instance of entity type 'Dato' cannot be tracked because another instance with the same key value…”*.
- **Menús**:
  - Asegúrate de `await` en las llamadas asíncronas (si pulsas “3” y “no pasa nada”, suele ser un `await` faltante o un `break` perdido).
- **Matches**:
  - Consulta por “pares” recíprocos (A → B like **y** B → A like).

---

## ▶️ Cómo ejecutar

1. Configura tu cadena de conexión (por ejemplo en `appsettings.json` o donde lo tengas).
2. Restaura paquetes y ejecuta:

3. Flujo sugerido para probar:
- Crea 2 usuarios (A y B) e inicia sesión con A.
- Completa el **perfil** de A y B.
- Desde A: **Ver Perfiles** → da **like** a B.
- Inicia sesión con B → **Ver Perfiles** → da **like** a A.
- Ve a **Ver matches**: debería aparecer el match A ❤ B.
- Revisa **estadísticas** 📊.


---

## 🗺️ Roadmap

- Reset diario de `likes_disponibles`.
- Mensajería entre matches.
- Filtros por intereses/edad/género.
- Exportar estadísticas / reportes.

---

## 👥 Autores

- **Juliana Andrea Pallares Novoa**  
- **Eduardo Elias Castellanos Picon**

