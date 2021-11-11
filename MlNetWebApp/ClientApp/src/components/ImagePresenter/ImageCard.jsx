import React from 'react';
import {
    Card, CardImg, CardText, CardBody,
    CardTitle, CardSubtitle, Button
} from 'reactstrap';

const ImageCard = (props) =>
{
    return (
        <div>

            <Card>
                <CardImg top width="100%" src={`data:image/jpeg;base64,${props.imageData.base64ImageContent}`} alt="analyzed image" />
                <CardBody>
                    <CardTitle tag="h5">Tags</CardTitle>
                    <CardSubtitle tag="h6" className="mb-2 text-muted">Analyis Result</CardSubtitle>
                    <CardText>
                        <ul>
                            {
                                props.imageData.imageAnalysisResults.map((rst,index) => {
                                    return (<li key={index}><div><p>{'Confidence level:'+(rst.predictionConfidence*100)+'%'}- {rst.label}</p></div></li>)
                                })
                            }
                        </ul>
                    </CardText>
                </CardBody>
            </Card>
        </div>
    );
}

export default ImageCard;