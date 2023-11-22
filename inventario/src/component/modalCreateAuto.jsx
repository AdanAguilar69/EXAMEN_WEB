import React,{useState} from 'react';
import { Button, Modal, Backdrop, Fade, TextField } from '@mui/material';

const ModalCreateAuto = ({ isOpen, onClose, onCreateProduct }) => {
    const [marca, setMarca] = useState('');
    const [modelo, setModelo] = useState('');
    const [color, setColor] = useState('');
  
    const handleCreateProduct = () => {
      const newProduct = {
        marca,
        modelo,
        color,
      };
  
      onCreateProduct(newProduct);
  
      setMarca('');
      setModelo('');
      setColor('');
    };
  
    return (
      <Modal
        open={isOpen}
        onClose={onClose}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Fade in={isOpen}>
          <div style={{ 
            position: 'absolute',
            top: '50%',
            left: '50%',
            transform: 'translate(-50%, -50%)',
            backgroundColor: '#fff',
            padding: '20px',
            borderRadius: '8px',
            width: '300px',
            textAlign: 'center',
          }}>
            <h2>Nuevo Auto</h2>
            <TextField
              label="Marca"
              variant="outlined"
              value={marca}
              onChange={(e) => setMarca(e.target.value)}
              style={{ marginBottom: '10px', width: '100%' }}
            />
            <br />
            <TextField
              label="Modelo"
              variant="outlined"
              value={modelo}
              onChange={(e) => setModelo(e.target.value)}
              style={{ marginBottom: '10px', width: '100%' }}
            />
            <br />
            <TextField
              label="Color"
              variant="outlined"
              value={color}
              onChange={(e) => setColor(e.target.value)}
              style={{ marginBottom: '10px', width: '100%' }}
            />
            <Button variant="contained" color="primary" onClick={handleCreateProduct}>
              Agregar Auto
            </Button>
          </div>
        </Fade>
      </Modal>
    );
  };
  
  export default ModalCreateAuto;
