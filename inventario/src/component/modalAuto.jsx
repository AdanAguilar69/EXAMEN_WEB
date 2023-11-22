import { useState } from 'react';
import {
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
} from '@mui/material';


export const UpdateModal = ({ open, handleClose, handleUpdate, item }) => {
    const [updatedItem, setUpdatedItem] = useState({ ...item });
  
    const handleChange = (e) => {
      const { name, value } = e.target;
      setUpdatedItem((prevItem) => ({
        ...prevItem,
        [name]: value,
      }));
    };
  
    return (
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Actualizar Auto</DialogTitle>
        <DialogContent>
          <TextField
            label="Marca"
            name="marca"
            value={updatedItem.marca}
            onChange={handleChange}
            fullWidth
            margin="dense"
          />
          <TextField
            label="Modelo"
            name="modelo"
            value={updatedItem.modelo}
            onChange={handleChange}
            fullWidth
            margin="dense"
          />
          <TextField
            label="Color"
            name="color"
            value={updatedItem.color}
            onChange={handleChange}
            fullWidth
            margin="dense"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Cancelar
          </Button>
          <Button onClick={() => handleUpdate(updatedItem)} color="primary">
            Actualizar
          </Button>
        </DialogActions>
      </Dialog>
    );
  };
  