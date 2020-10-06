import {
  IonButtons,
  IonContent,
  IonHeader,
  IonMenuButton,
  IonPage,
  IonTitle,
  IonToolbar,
} from "@ionic/react";
import React from "react";
import { useParams } from "react-router";
import ExploreContainer from "../components/ExploreContainer";
import "./Page.css";

const Home: React.FC = () => {
  return (
    <IonPage>
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="start">
            <IonMenuButton />
          </IonButtons>
          <IonTitle>Home Header</IonTitle>
        </IonToolbar>
      </IonHeader>

      <IonContent>
        <IonHeader collapse="condense">
          <IonToolbar>
            <IonTitle size="large">The Home Page</IonTitle>
          </IonToolbar>
        </IonHeader>
        <div className="container">
          <strong>The Home Page</strong>
        </div>
      </IonContent>
    </IonPage>
  );
};

export default Home;
