import React, { useState } from "react";
import { Modal, Box, TextField, Button, Checkbox, FormControlLabel, Typography } from "@mui/material";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFishFarm, uploadFishFarmImage } from "../api/fishFarmApi";

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    borderRadius: 2,
    boxShadow: 24,
    p: 4,
};

export default function FishFarmFormModal({ onClose, onSuccess }) {
    const queryClient = useQueryClient();
    const [imageFile, setImageFile] = useState(null);
    const { register, handleSubmit } = useForm();

    const createMutation = useMutation({
        mutationFn: createFishFarm,
        onSuccess: () => queryClient.invalidateQueries(["fishFarms"]),
    });

    const onSubmit = async (data) => {
        if (!imageFile) {
            alert("Image is required!");
            return;
        }

        // create fish farm
        const created = await createMutation.mutateAsync(data);

        // upload image
        const imageUrl = await uploadFishFarmImage(imageFile, created.id);
        console.log("Image uploaded to:", imageUrl);

        onSuccess(); // close modal and refresh list
    };

    return (
        <Modal open={true} onClose={onClose}>
            <Box sx={{ ...style }}>
                <Typography color="text.primary" variant="h6" component="h2" align="center" gutterBottom>
                    Register fish farm
                </Typography>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <TextField label="Name" {...register("name")} required fullWidth margin="normal" />
                    <TextField
                        label="GPS Latitude"
                        type="number"
                        {...register("gpsLatitude", { valueAsNumber: true })}
                        required
                        fullWidth
                        margin="normal"
                        inputProps={{ step: 0.0001 }}
                    />
                    <TextField
                        label="GPS Longitude"
                        type="number"
                        {...register("gpsLongitude", { valueAsNumber: true })}
                        required
                        fullWidth
                        margin="normal"
                        inputProps={{ step: 0.0001 }}
                    />
                    <TextField
                        label="Number of cages"
                        type="number"
                        {...register("numOfCages", { valueAsNumber: true })}
                        required
                        fullWidth
                        margin="normal"
                    />
                    <FormControlLabel control={<Checkbox {...register("hasBarge")} />} label="Has barge?" />

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
                        Create Fish Farm
                    </Button>
                </form>
            </Box>
        </Modal>
    );
}
