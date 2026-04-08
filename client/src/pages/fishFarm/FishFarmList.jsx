import { useState } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { fetchFishFarms } from "../../api/fishFarmApi";
import WorkerListModal from "../../components/WorkerListModal";
import WorkerFormModal from "../../components/WorkerFormModal";
import FishFarmFormModal from "../../components/FishFarmFormModal";
import {
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Pagination
} from "@mui/material";
import { config } from "../../config";
import noImg from "../../assets/no-img.png";

export default function FishFarmList() {
  const [page, setPage] = useState(0);
  const [selectedFarm, setSelectedFarm] = useState(null);

  const [isWorkerListOpen, setIsWorkerListOpen] = useState(false);
  const [isWorkerFormOpen, setIsWorkerFormOpen] = useState(false);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const queryClient = useQueryClient();

  const { data, isLoading, isError } = useQuery({
    queryKey: ["fishFarms", page],
    queryFn: () =>
      fetchFishFarms(page * config.defaultPageSize, config.defaultPageSize),
    keepPreviousData: true
  });

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p>Error loading fish farms</p>;

  return (
    <>
      <div>
        <Typography
          color="text.primary"
          variant="h3"
          component="h6"
          align="center"
          gutterBottom
        >
          Aqua Farm Management
        </Typography>
        <Button
          variant="contained"
          color="primary"
          onClick={() => setIsModalOpen(true)}
          style={{ marginBottom: 16 }}
        >
          Register Aqua Farm
        </Button>

        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>GPS Position</TableCell>
                <TableCell>Number of Cages</TableCell>
                <TableCell>Has Barge</TableCell>
                <TableCell>Picture</TableCell>
                <TableCell>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data.items.slice(0, config.defaultPageSize).map((farm) => (
                <TableRow key={farm.id}>
                  <TableCell>{farm.name}</TableCell>
                  <TableCell>{`${farm.gpsLatitude.toFixed(
                    4
                  )}, ${farm.gpsLongitude.toFixed(4)}`}</TableCell>
                  <TableCell>{farm.numOfCages}</TableCell>
                  <TableCell>{farm.hasBarge ? "Yes" : "No"}</TableCell>
                  <TableCell>
                    <img
                      src={farm.picture ? config.backendUrl + farm.picture : noImg}
                      alt={farm.name}
                      style={{
                        width: 80,
                        height: 60,
                        objectFit: "cover",
                        borderRadius: 4
                      }}
                    />
                  </TableCell>

                  <TableCell>
                    {/* View Workers */}
                    <Button
                      size="small"
                      variant="outlined"
                      onClick={() => {
                        setSelectedFarm(farm);
                        setIsWorkerListOpen(true);
                      }}
                      style={{ marginRight: 8 }}
                    >
                      View Workers
                    </Button>

                    {/* Add Worker */}
                    <Button
                      size="small"
                      variant="contained"
                      onClick={() => {
                        setSelectedFarm(farm);
                        setIsWorkerFormOpen(true);
                      }}
                    >
                      Add Worker
                    </Button>
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

          {/* Worker List Modal */}
          {isWorkerListOpen && selectedFarm && (
            <WorkerListModal
              farm={selectedFarm}
              onClose={() => {
                setIsWorkerListOpen(false);
                setSelectedFarm(null);
              }}
            />
          )}

          {/* Worker Registration Modal */}
          {isWorkerFormOpen && selectedFarm && (
            <WorkerFormModal
              farm={selectedFarm}
              onClose={() => {
                setIsWorkerFormOpen(false);
                setSelectedFarm(null);
              }}
              onSuccess={() => {
                setIsWorkerFormOpen(false);
                queryClient.invalidateQueries(["workers", selectedFarm.id]);
              }}
            />
          )}

          {/* FishFarm Registration Modal */}
          {isModalOpen && (
            <FishFarmFormModal
              onClose={() => setIsModalOpen(false)}
              onSuccess={() => {
                setIsModalOpen(false);
                queryClient.invalidateQueries(["fishFarms"]);
              }}
            />
          )}
        </TableContainer>
      </div>

    </>
  );
}
