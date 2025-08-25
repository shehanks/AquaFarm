import { useState } from "react";
import {
  Modal,
  Box,
  TextField,
  Button,
  Typography,
  MenuItem,
} from "@mui/material";
import { Controller, useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createWorker, uploadWorkerImage } from "../api/workerApi";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  borderRadius: 2,
  boxShadow: 24,
  p: 4,
};

export default function WorkerFormModal({ farm, onClose, onSuccess }) {
  console.log("WorkerFormModal for farm:", farm);
  const queryClient = useQueryClient();
  const [imageFile, setImageFile] = useState(null);
  const { register, handleSubmit, control } = useForm({
    defaultValues: {
      position: "Worker",
      certifiedUntil: "",
    },
  });

  const createMutation = useMutation({
    mutationFn: createWorker,
    onSuccess: () => queryClient.invalidateQueries(["workers", farm.id]),
  });

  const onSubmit = async (data) => {
    const certifiedUntilUTC = new Date(data.certifiedUntil + "T00:00:00Z").toISOString();

    const created = await createMutation.mutateAsync({
      ...data,
      certifiedUntil: certifiedUntilUTC,
      fishFarmId: farm.id,
    });

    if (imageFile) {
      const imageUrl = await uploadWorkerImage(imageFile, created.id);
      console.log("Worker image uploaded to:", imageUrl);
    }

    onSuccess();
  };

  return (
    <Modal open={true} onClose={onClose}>
      <Box sx={{ ...style }}>
        <Typography
          color="text.primary"
          variant="h6"
          component="h2"
          align="center"
          gutterBottom
        >
          Register Worker for {farm.name}
        </Typography>
        <form onSubmit={handleSubmit(onSubmit)}>
          <TextField
            label="Full Name"
            {...register("name")}
            required
            fullWidth
            margin="normal"
          />
          <TextField
            label="Age"
            type="number"
            {...register("age", { valueAsNumber: true })}
            required
            fullWidth
            margin="normal"
          />
          <TextField
            label="Email"
            type="email"
            {...register("email")}
            required
            fullWidth
            margin="normal"
          />

          <Controller
            name="position"
            control={control}
            defaultValue="Worker"
            render={({ field }) => (
              <TextField
                label="Position"
                select
                required
                fullWidth
                margin="normal"
                {...field}
              >
                <MenuItem value="CEO">CEO</MenuItem>
                <MenuItem value="Worker">Worker</MenuItem>
                <MenuItem value="Captain">Captain</MenuItem>
              </TextField>
            )}
          />

          <TextField
            label="Certified Until (UTC)"
            type="date"
            {...register("certifiedUntil")}
            required
            fullWidth
            margin="normal"
            InputLabelProps={{ shrink: true }}
          />

          {/* IMAGE INPUT */}
          <Box mt={2} mb={2}>
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setImageFile(e.target.files[0])}
            />
            {imageFile && (
              <Typography variant="body2" mt={1} color="text.secondary">
                {imageFile.name}
              </Typography>
            )}
          </Box>

          <Button type="submit" variant="contained" fullWidth>
            Add
          </Button>
        </form>
      </Box>
    </Modal>
  );
}
