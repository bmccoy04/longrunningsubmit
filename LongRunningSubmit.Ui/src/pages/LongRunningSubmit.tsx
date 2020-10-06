import {
  IonButtons,
  IonContent,
  IonHeader,
  IonMenuButton,
  IonPage,
  IonTitle,
  IonToolbar,
  IonItemDivider,
  IonInput,
  IonLabel,
  IonItem,
  IonButton,
  IonToast
} from "@ionic/react";
import React, { useState, useEffect } from "react";
import { useParams } from "react-router";
import ExploreContainer from "../components/ExploreContainer";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "./Page.css";

const LongRunningSubmit: React.FC = () => {
  const [weatherForecasts, setWeatherForecasts] = useState<Array<any>>([]); //I'm a bad person
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<any>(null);
  const [state, setState] = useState<string>("dormant");
  const [connection, setConnection] = useState<any>({});
  const [submission, setSubmission] = useState<any>({
    firstName: "",
    lastName: "",
    somethingElse: "",
  });

  //   connection.on("sendMessage", (data) => {
  //     console.log(data);

  //   });

  useEffect(() => {
    console.log("--- connection being built ---");
    const connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/hub/notifications")
      .build();
    
      connection.start().then(() => console.log("--- connection started ----"));

      connection.on("sendMessage", (name, message) =>{ console.log("Complete"); setState("Complete") });

      setConnection(connection);
  }, []);

  const submit = () => {
    setState("Sending");
    console.log("Sending");
    fetch("https://localhost:5001/api/long-running-submit-controller", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(submission),
    }).then(response => {setState("Pending"); console.log("Pending");});

    //connection.invoke("sendMessage", "bryan", "message sent from web");
  };

  return (
    <IonPage>
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="start">
            <IonMenuButton />
          </IonButtons>
          <IonTitle>Weather Forecast Header</IonTitle>
        </IonToolbar>
      </IonHeader>

      <IonContent>
        <IonHeader collapse="condense">
          <IonToolbar>
            <IonTitle size="large">The Weather Forecast Page</IonTitle>
          </IonToolbar>
        </IonHeader>
        <div>
          <strong>Long Running Submit Page</strong>
          <IonItem>
            <IonLabel>First Name:</IonLabel>
            <IonInput
              value={submission.firstName}
              placeholder="First Name"
              onIonChange={(e) =>
                setSubmission({ ...submission, firstName: e.detail.value! })
              }
              clearInput
            ></IonInput>
          </IonItem>
          <IonItem>
            <IonLabel>Last Name:</IonLabel>
            <IonInput
              value={submission.lastName}
              placeholder="Last Name"
              onIonChange={(e) =>
                setSubmission({ ...submission, lastName: e.detail.value! })
              }
              clearInput
            ></IonInput>
          </IonItem>
          <IonItem>
            <IonLabel>Something else:</IonLabel>
            <IonInput
              value={submission.somethingElse}
              placeholder="Something Else"
              onIonChange={(e) =>
                setSubmission({ ...submission, somethingElse: e.detail.value! })
              }
              clearInput
            ></IonInput>
          </IonItem>
          <IonButton color="primary" onClick={submit}>
            Primary
          </IonButton>
          <h3>State: {state}</h3>
        </div>

      </IonContent>
    </IonPage>
  );
};

export default LongRunningSubmit;
