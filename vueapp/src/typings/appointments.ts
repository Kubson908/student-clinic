export type Appointments = {
  appointments: Array<{
    controlAppointment: Object;
    date: string;
    doctor: Object;
    finished: boolean;
    id: number;
    medicines: string;
    recommendations: string;
    symptoms: string;
  }>;
};
