import { useEffect, useState } from 'react'
import { useInven } from './Context/CrudContext'
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
} from '@mui/material';
import { UpdateModal } from './component/modalAuto';
import ModalCreateAuto from './component/modalCreateAuto';


function App() {
  const {
    getallAutos,
    deleteAuto,
    updateAuto,
    createAuto,
  } = useInven()

  const [autos, setAutos] = useState([]);


  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
 

  const [selectedAuto, setSelectedAuto] = useState({});


  const [isCreateModalOpen, setCreateModalOpen] = useState(false);
  

  const tableContainerStyle = {
    margin: '20px auto', // Ajusta el margen y centra la tabla
    maxWidth: '800px',    // Establece el ancho máximo de la tabla
  };


  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getallAutos()
        setAutos(response)
      } catch (error) {

      }
    }
    fetchData()
  }, [getallAutos, autos])

  const handleDeleteAuto = async (id) => {
    try {
      await deleteAuto(id);
    } catch (error) {
      console.error('Error al eliminar auto', error);
    }
  };

  const handleOpenUpdateModal = (auto) => {
    setSelectedAuto(auto);
    setIsUpdateModalOpen(true);
  };

  const handleCloseUpdateModal = () => {
    setIsUpdateModalOpen(false);
  };

  const handleUpdateAuto = async (updatedAuto) => {
    await updateAuto(selectedAuto.iD_Automovil, updatedAuto);
    handleCloseUpdateModal();
  };

  const handleOpenCreateModal = () => {
    setCreateModalOpen(true);
  };

  const handleCloseCreateModal = () => {
    setCreateModalOpen(false);
  };

  const handleCreateAuto = async (newAuto) => {
    try {
      await createAuto(newAuto);
    } catch (error) {
      console.error('Error al crear el auto:', error);
    }
    handleCloseCreateModal();
  };



  const renderCreateButton = () => (
    <Button variant="contained" color="primary" onClick={handleOpenCreateModal}>
      Nuevo Auto
    </Button>
  );

  return (
    <>
      <h2 style={{ textAlign: 'center', color: "blue" }} >Cantidad de Autos</h2>
      <div style={{ textAlign: 'center', display: 'flex', fontSize: '24px', justifyContent: 'center', alignItems: 'center', gap: '125px' }}>
        <p> <span style={{ color: 'blue' }}>{autos.length}</span></p>
      </div>
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>

        {renderCreateButton()}
      </div>
      <TableContainer component={Paper} style={tableContainerStyle}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Marca</TableCell>
              <TableCell>Modelo</TableCell>
              <TableCell>color</TableCell>
              <TableCell>Acciones</TableCell>
            </TableRow>
          </TableHead>
          {autos?.map((auto, index) => (
            <TableBody key={index}>
              <TableRow>
                <TableCell>{auto.marca}</TableCell>
                <TableCell>{auto.modelo}</TableCell>
                <TableCell>{auto.color}</TableCell>
                <TableCell>
                  <Button
                    variant="outlined"
                    color="primary"
                    onClick={() => handleOpenUpdateModal(auto)}
                  >
                    Actualizar
                  </Button>
                  <Button
                    variant="outlined"
                    color="secondary"
                    onClick={() => handleDeleteAuto(auto.iD_Automovil)}
                  >
                    Eliminar
                  </Button>
                </TableCell>
              </TableRow>
            </TableBody>
          ))}
        </Table>
      </TableContainer>

      <UpdateModal
        open={isUpdateModalOpen}
        handleClose={handleCloseUpdateModal}
        handleUpdate={handleUpdateAuto}
        item={selectedAuto}
      />

      <ModalCreateAuto
        isOpen={isCreateModalOpen}
        onClose={handleCloseCreateModal}
        onCreateProduct={handleCreateAuto}
      />


    </>
  )
}

export default App
