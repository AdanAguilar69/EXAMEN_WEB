import { createContext, useContext} from 'react';
import axios from 'axios';

const CrudContext = createContext();

export const CrudProvider = ({children}) =>{


    const getallAutos = async () => {
        try {
          const response = await axios.get('https://localhost:44335/api/Auto');
    
          if (response.data) {
            return response.data;
          } else {
            console.error('No se pudieron obtener todos los autos');
            return [];
          }
        } catch (error) {
          console.error('Error al obtener proyectos:', error);
          return [];
        }
      };
      
    


      const deleteAuto = async (AutoID) => {
        try {
          const response = await axios.delete(`https://localhost:44335/api/Auto/${AutoID}`);
    
          if (response.data) {
            return response.data;
          } else {
            console.error('No se pudo eliminar el proyecto.');
            return null;
          }
        } catch (error) {
          console.error('Error al eliminar el proyecto:', error);
          return null;
        }
      };
     
     

    const updateAuto = async (autoid, updateAutoData) => {
      try {
        
        const response = await axios.put(`https://localhost:44335/api/Auto/${autoid}`, updateAutoData);
  
        return response.data; 
      } catch (error) {
        // Maneja los errores según tus necesidades
        console.error('Error al actualizar el proyecto:', error);
        throw error; 
      }
      
    };
    

      const createAuto = async (AutoData) => {
        try {
          // Realiza la solicitud al servidor para crear un nuevo auto
          const response = await axios.post(`https://localhost:44335/api/Auto`, AutoData);

          return response.data; // Retorna el auto creado si es necesario
        } catch (error) {
          // Maneja los errores según tus necesidades
          console.error('Error al crear el proyecto:', error);
          throw error; // Puedes lanzar el error para que el componente que llama pueda manejarlo
        }
      };

      


    
    return (
        <CrudContext.Provider value={{
                getallAutos,
                deleteAuto,
                updateAuto,
                createAuto,
               }}>
          {children}
        </CrudContext.Provider>
      );
}
export const useInven = () => {
    return useContext(CrudContext);
  };