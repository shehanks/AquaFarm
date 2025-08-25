import React, { useState } from "react";
import {
  Modal,
  Box,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Pagination
} from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { fetchWorkersByFishFarm } from "../api/fishFarmApi";
import { config } from "../config";
import noImg from "../assets/no-img.png";

const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 600,
  bgcolor: 'background.paper',
  borderRadius: 2,
  boxShadow: 24,
  p: 4,
};

export default function WorkerListModal({ farm, onClose }) {
  const [page, setPage] = useState(0);

  const { data, isLoading, isError } = useQuery({
    queryKey: ["workers", farm.id, page],
    queryFn: () => fetchWorkersByFishFarm(farm.id, page * config.defaultPageSize, config.defaultPageSize),
    keepPreviousData: true
  });

  if (isLoading) return <Modal open={true} onClose={onClose}><Box sx={style}>Loading...</Box></Modal>;
  if (isError) return <Modal open={true} onClose={onClose}><Box sx={style}>Error loading workers</Box></Modal>;

  return (
    <Modal open={true} onClose={onClose}>
      <Box sx={style}>
        <Typography
          color="text.primary"
          variant="h6"
          component="h2"
          align="center"
          gutterBottom
        >
          Workers for {farm.name}
        </Typography>
        <TableContainer component={Paper} sx={{ width: "100%", overflowX: "auto" }}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>Age</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Position</TableCell>
                <TableCell>Certified Until (UTC)</TableCell>
                <TableCell>Picture</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data.items.slice(0, config.defaultPageSize).map(worker => (
                <TableRow key={worker.id}>
                  <TableCell>{worker.name}</TableCell>
                  <TableCell>{worker.age}</TableCell>
                  <TableCell sx={{ wordBreak: "break-all", maxWidth: 200 }}>
                    {worker.email}
                  </TableCell>
                  <TableCell>{worker.position}</TableCell>
                  <TableCell>
                    {worker.certifiedUntil
                      ? new Date(worker.certifiedUntil).toISOString().slice(0, 10)
                      : ""}
                  </TableCell>
                  <TableCell>
                    <img
                      src={worker.picture ? config.backendUrl + worker.picture : noImg}
                      alt={worker.name}
                      style={{ width: 60, height: 40, objectFit: 'cover', borderRadius: 4 }}
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div style={{ display: "flex", justifyContent: "center", margin: "10px 0" }}>
            <Pagination
              count={Math.ceil(data.meta.totalCount / config.defaultPageSize)}
              page={data.meta.page}
              onChange={(event, value) => setPage(value - 1)}
              color="primary"
              shape="rounded"
            />
          </div>
        </TableContainer>
      </Box>
    </Modal>
  );
}
